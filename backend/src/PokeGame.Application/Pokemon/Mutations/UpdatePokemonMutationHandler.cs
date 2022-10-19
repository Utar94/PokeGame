using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class UpdatePokemonMutationHandler : SavePokemonMutationHandler, IRequestHandler<UpdatePokemonMutation, PokemonModel>
  {
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public UpdatePokemonMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    ) : base(querier, repository)
    {
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(UpdatePokemonMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await Repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      UpdatePokemonPayload payload = request.Payload;

      await EnsureHeldItemExistsAsync(payload, cancellationToken);
      await EnsureTrainerExistsAndPositionIsFreeAsync(payload, pokemon.Id, cancellationToken);
      await EnsureMovesAreValidAsync(payload, cancellationToken);

      if (payload.OriginalTrainerId.HasValue && await Repository.LoadAsync<Trainer>(payload.OriginalTrainerId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Trainer>(payload.OriginalTrainerId.Value, nameof(payload.OriginalTrainerId));
      }

      pokemon.Update(payload);
      _validator.ValidateAndThrow(pokemon);

      await Repository.SaveAsync(pokemon, cancellationToken);

      return await Querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
