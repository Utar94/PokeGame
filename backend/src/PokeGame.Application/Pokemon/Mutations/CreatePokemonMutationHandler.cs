using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
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
      await EnsureMovesAreValidAsync(payload, cancellationToken);

      var pokemon = new Domain.Pokemon.Pokemon(payload, species);
      _validator.ValidateAndThrow(pokemon);

      await Repository.SaveAsync(pokemon, cancellationToken);

      return await Querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
