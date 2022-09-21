using FluentValidation;
using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class CatchPokemonMutationHandler : IRequestHandler<CatchPokemonMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public CatchPokemonMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(CatchPokemonMutation request, CancellationToken cancellationToken)
    {
      CatchPokemonPayload payload = request.Payload;

      Trainer trainer = await _repository.LoadAsync<Trainer>(payload.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(payload.TrainerId, nameof(payload.TrainerId));

      PokemonPosition position = await FindFirstAvailablePositionAsync(trainer.Id, cancellationToken);

      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      if (payload.Heal != null)
      {
        pokemon.Heal(payload.Heal);
      }
      pokemon.Catch(payload.Location, trainer.Id, position.Position, position.Box, payload.Surname);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }

    private async Task<PokemonPosition> FindFirstAvailablePositionAsync(Guid trainerId, CancellationToken cancellationToken)
    {
      ListModel<PokemonModel> trainerPokemon = await _querier
        .GetPagedAsync(trainerId: trainerId, cancellationToken: cancellationToken);

      HashSet<string> positions = trainerPokemon.Items.Where(x => x.Position.HasValue)
        .Select(x => x.Box.HasValue ? string.Join('_', x.Box.Value, x.Position!.Value) : x.Position!.Value.ToString()).ToHashSet();

      for (byte position = 0; position <= 5; position++)
      {
        if (!positions.Contains(position.ToString()))
        {
          return new(position);
        }
      }

      for (byte box = 0; box <= 31; box++)
      {
        for (byte position = 0; position <= 29; position++)
        {
          if (!positions.Contains($"{box}_{position}"))
          {
            return new(position, box);
          }
        }
      }

      throw new NoAvailablePositionException(trainerId);
    }
  }
}
