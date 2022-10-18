using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Mutations
{
  internal class HealTrainerPartyMutationHandler : IRequestHandler<HealTrainerPartyMutation>
  {
    private readonly IPokemonQuerier _pokemonQuerier;
    private readonly IValidator<Domain.Pokemon.Pokemon> _pokemonValidator;
    private readonly IRepository _repository;

    public HealTrainerPartyMutationHandler(
      IPokemonQuerier pokemonQuerier,
      IValidator<Domain.Pokemon.Pokemon> pokemonValidator,
      IRepository repository
    )
    {
      _pokemonQuerier = pokemonQuerier;
      _pokemonValidator = pokemonValidator;
      _repository = repository;
    }

    public async Task<Unit> Handle(HealTrainerPartyMutation request, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.Id);

      IEnumerable<Guid> ids = (await _pokemonQuerier.GetPagedAsync(inParty: true, trainerId: trainer.Id, cancellationToken: cancellationToken))
        .Items.Select(x => x.Id).Distinct();
      if (ids.Any())
      {
        IEnumerable<Domain.Pokemon.Pokemon> party = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(ids, cancellationToken);

        IEnumerable<Guid> moveIds = party.SelectMany(x => x.Moves.Select(y => y.MoveId)).Distinct();
        Dictionary<Guid, Move> moves = (await _repository.LoadAsync<Move>(moveIds, cancellationToken))
          .ToDictionary(x => x.Id, x => x);

        foreach (Domain.Pokemon.Pokemon pokemon in party)
        {
          HealPokemonPayload payload = new()
          {
            RestoreAllPowerPoints = true,
            RestoreHitPoints = 999,
            RemoveAllConditions = true
          };

          List<Move> pokemonMoves = new(capacity: pokemon.Moves.Count);
          foreach (PokemonMove pokemonMove in pokemon.Moves)
          {
            if (moves.TryGetValue(pokemonMove.MoveId, out Move? move))
            {
              pokemonMoves.Add(move);
            }
          }

          pokemon.Heal(payload, pokemonMoves);
          _pokemonValidator.ValidateAndThrow(pokemon);
        }

        await _repository.SaveAsync(party, cancellationToken);
      }

      return Unit.Value;
    }
  }
}
