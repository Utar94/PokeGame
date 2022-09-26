using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class PokemonGainMutationHandler : IRequestHandler<PokemonGainMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public PokemonGainMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(PokemonGainMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      bool isHoldingSootheBell = false;
      if (pokemon.HeldItemId.HasValue)
      {
        Item heldItem = await _repository.LoadAsync<Item>(pokemon.HeldItemId.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Item>(pokemon.HeldItemId.Value);

        isHoldingSootheBell = heldItem.Name == "Soothe Bell";
      }

      pokemon.GainedExperience(request.Payload, isHoldingSootheBell);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
