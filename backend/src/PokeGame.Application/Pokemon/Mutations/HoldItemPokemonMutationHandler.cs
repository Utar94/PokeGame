using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class HoldItemPokemonMutationHandler : IRequestHandler<HoldItemPokemonMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _pokemonValidator;
    private readonly IValidator<Trainer> _trainerValidator;

    public HoldItemPokemonMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> pokemonValidator,
      IValidator<Trainer> trainerValidator
    )
    {
      _querier = querier;
      _repository = repository;
      _pokemonValidator = pokemonValidator;
      _trainerValidator = trainerValidator;
    }

    public async Task<PokemonModel> Handle(HoldItemPokemonMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id, nameof(request.Id));

      if (pokemon.History == null)
      {
        throw new TrainerIsRequiredException(pokemon);
      }

      Trainer trainer = await _repository.LoadAsync<Trainer>(pokemon.History.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(pokemon.History.TrainerId);

      Item? item = null;
      if (request.ItemId.HasValue)
      {
        item = await _repository.LoadAsync<Item>(request.ItemId.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Item>(request.ItemId.Value, nameof(request.ItemId));

        trainer.RemoveItem(item, quantity: 1);
      }
      if (pokemon.HeldItemId.HasValue)
      {
        Item heldItem = await _repository.LoadAsync<Item>(pokemon.HeldItemId.Value, cancellationToken)
          ?? throw new EntityNotFoundException<Item>(pokemon.HeldItemId.Value);

        trainer.AddItem(heldItem, quantity: 1);
      }
      _trainerValidator.ValidateAndThrow(trainer);
      await _repository.SaveAsync(trainer, cancellationToken);

      pokemon.HoldItem(item);
      _pokemonValidator.ValidateAndThrow(pokemon);
      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
