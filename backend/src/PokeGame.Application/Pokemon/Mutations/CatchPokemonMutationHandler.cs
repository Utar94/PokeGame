using FluentValidation;
using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
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

      Item ball = await _repository.LoadAsync<Item>(payload.BallId, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(payload.BallId, nameof(payload.BallId));

      Trainer trainer = await _repository.LoadAsync<Trainer>(payload.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(payload.TrainerId, nameof(payload.TrainerId));

      PokemonPosition position = await FindFirstAvailablePositionAsync(trainer.Id, cancellationToken);

      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      if (payload.Heal != null || position.Box.HasValue)
      {
        HealPokemonPayload healPayload = payload.Heal ?? new()
        {
          RemoveAllConditions = true,
          RestoreAllPowerPoints = true,
          RestoreHitPoints = 999
        };

        IEnumerable<Guid> moveIds = pokemon.Moves.Select(x => x.MoveId);
        IEnumerable<Move> moves = await _repository.LoadAsync<Move>(moveIds, cancellationToken);
        pokemon.Heal(healPayload, moves);
      }
      pokemon.Catch(ball.Id, payload.Location, trainer.Id, position.Position, position.Box, payload.Friendship, payload.Surname);
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

      for (byte position = 1; position <= 6; position++)
      {
        if (!positions.Contains(position.ToString()))
        {
          return new(position);
        }
      }

      for (byte box = 1; box <= 32; box++)
      {
        for (byte position = 1; position <= 30; position++)
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
