using FluentValidation;
using MediatR;
using PokeGame.Application.Moves;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class CreatePokemonMutationHandler : IRequestHandler<CreatePokemonMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public CreatePokemonMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(CreatePokemonMutation request, CancellationToken cancellationToken)
    {
      CreatePokemonPayload payload = request.Payload;

      Domain.Species.Species species = await _repository.LoadAsync<Domain.Species.Species>(payload.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(payload.SpeciesId, nameof(payload.SpeciesId));

      if (!species.AbilityIds.Contains(payload.AbilityId))
      {
        throw new InvalidAbilityException(species, payload.AbilityId);
      }

      if (payload.HeldItemId.HasValue && await _repository.LoadAsync<Item>(payload.HeldItemId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Item>(payload.HeldItemId.Value, nameof(payload.HeldItemId));
      }

      if (payload.Moves != null)
      {
        IEnumerable<Guid> moveIds = payload.Moves.Select(x => x.MoveId);
        Dictionary<Guid, Move> moves = (await _repository.LoadAsync<Move>(moveIds, cancellationToken))
          .ToDictionary(x => x.Id, x => x);

        var missingIds = new List<Guid>(capacity: moveIds.Count());

        foreach (PokemonMovePayload movePayload in payload.Moves)
        {
          if (!moves.TryGetValue(movePayload.MoveId, out Move? move))
          {
            missingIds.Add(movePayload.MoveId);

            continue;
          }
          else if (movePayload.RemainingPowerPoints > move.PowerPoints)
          {
            throw new RemainingPowerPointsExceededException(move, movePayload.RemainingPowerPoints);
          }
        }

        if (missingIds.Any())
        {
          throw new MovesNotFoundException(missingIds);
        }
      }

      if (payload.History != null && await _repository.LoadAsync<Trainer>(payload.History.TrainerId, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Trainer>(payload.History.TrainerId, $"{nameof(payload.History)}.{nameof(payload.History.TrainerId)}");
      }

      var pokemon = new Domain.Pokemon.Pokemon(payload, species);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
