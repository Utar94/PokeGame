using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class SwapPokemonMutationHandler : IRequestHandler<SwapPokemonMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public SwapPokemonMutationHandler(IPokemonQuerier querier, IRepository repository, IValidator<Domain.Pokemon.Pokemon> validator)
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(SwapPokemonMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id, nameof(request.Id));

      Domain.Pokemon.Pokemon other = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.OtherId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.OtherId, nameof(request.OtherId));

      if (!pokemon.Equals(other))
      {
        if (pokemon.History == null || other.History == null || pokemon.History.TrainerId != other.History.TrainerId)
        {
          throw new CannotSwapPokemonException(pokemon, other);
        }

        PokemonPosition firstPosition = pokemon.Position ?? throw new InvalidOperationException($"The Pokémon '{pokemon.Id}' has no position.");
        PokemonPosition otherPosition = other.Position ?? throw new InvalidOperationException($"The Pokémon '{other.Id}' has no position.");

        pokemon.Move(null);
        await _repository.SaveAsync(pokemon, cancellationToken);

        other.Move(firstPosition);
        _validator.ValidateAndThrow(other);
        await _repository.SaveAsync(other, cancellationToken);

        pokemon.Move(otherPosition);
        _validator.ValidateAndThrow(pokemon);
        await _repository.SaveAsync(pokemon, cancellationToken);
      }

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
