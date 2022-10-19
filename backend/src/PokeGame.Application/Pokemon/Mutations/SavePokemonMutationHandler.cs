using PokeGame.Application.Moves;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal abstract class SavePokemonMutationHandler
  {
    protected SavePokemonMutationHandler(IPokemonQuerier querier, IRepository repository)
    {
      Querier = querier;
      Repository = repository;
    }

    protected IPokemonQuerier Querier { get; }
    protected IRepository Repository { get; }

    protected async Task EnsureHeldItemExistsAsync(SavePokemonPayload payload, CancellationToken cancellationToken)
    {
      if (payload.HeldItemId.HasValue && await Repository.LoadAsync<Item>(payload.HeldItemId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Item>(payload.HeldItemId.Value, nameof(payload.HeldItemId));
      }
    }

    protected async Task EnsureMovesAreValidAsync(SavePokemonPayload payload, CancellationToken cancellationToken)
    {
      if (payload.Moves != null)
      {
        IEnumerable<Guid> moveIds = payload.Moves.Select(x => x.MoveId);
        Dictionary<Guid, Move> moves = (await Repository.LoadAsync<Move>(moveIds, cancellationToken))
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
    }

    protected async Task EnsureTrainerExistsAndPositionIsFreeAsync(SavePokemonPayload payload, Guid? pokemonId, CancellationToken cancellationToken)
    {
      if (payload.History != null)
      {
        Trainer? trainer = await Repository.LoadAsync<Trainer>(payload.History.TrainerId, cancellationToken);
        if (trainer == null)
        {
          throw new EntityNotFoundException<Trainer>(payload.History.TrainerId, $"{nameof(payload.History)}.{nameof(payload.History.TrainerId)}");
        }

        if (payload.Position.HasValue)
        {
          var pokemonPosition = new PokemonPosition(payload.Position.Value, payload.Box);
          PokemonModel? pokemon = await Querier.GetAsync(trainer.Id, pokemonPosition, cancellationToken);
          if (pokemon != null && (!pokemonId.HasValue || pokemonId.Value != pokemon.Id))
          {
            throw new PokemonPositionAlreadyUsedException(trainer, pokemonPosition);
          }
        }
      }
    }
  }
}
