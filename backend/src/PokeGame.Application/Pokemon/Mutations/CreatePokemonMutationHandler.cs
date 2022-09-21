using FluentValidation;
using MediatR;
using PokeGame.Application.Moves;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class CreatePokemonMutationHandler : SavePokemonMutationHandler, IRequestHandler<CreatePokemonMutation, PokemonModel>
  {
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public CreatePokemonMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    ) : base(querier, repository)
    {
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(CreatePokemonMutation request, CancellationToken cancellationToken)
    {
      CreatePokemonPayload payload = request.Payload;

      Domain.Species.Species species = await Repository.LoadAsync<Domain.Species.Species>(payload.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(payload.SpeciesId, nameof(payload.SpeciesId));

      if (!species.AbilityIds.Contains(payload.AbilityId))
      {
        throw new InvalidAbilityException(species, payload.AbilityId);
      }

      await EnsureHeldItemExistsAsync(payload, cancellationToken);
      await EnsureTrainerExistsAndPositionIsFreeAsync(payload, pokemonId: null, cancellationToken);

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

      var pokemon = new Domain.Pokemon.Pokemon(payload, species);
      _validator.ValidateAndThrow(pokemon);

      await Repository.SaveAsync(pokemon, cancellationToken);

      return await Querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
