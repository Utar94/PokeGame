using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class UpdatePokemonConditionMutationHandler : IRequestHandler<UpdatePokemonConditionMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public UpdatePokemonConditionMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(UpdatePokemonConditionMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      pokemon.UpdateCondition(request.Payload);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
