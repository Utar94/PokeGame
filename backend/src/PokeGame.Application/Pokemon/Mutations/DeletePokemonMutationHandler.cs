using MediatR;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class DeletePokemonMutationHandler : IRequestHandler<DeletePokemonMutation>
  {
    private readonly IRepository<Domain.Pokemon.Pokemon> _repository;

    public DeletePokemonMutationHandler(IRepository<Domain.Pokemon.Pokemon> repository)
    {
      _repository = repository;
    }

    public async Task<Unit> Handle(DeletePokemonMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      pokemon.Delete();

      await _repository.SaveAsync(pokemon, cancellationToken);

      return Unit.Value;
    }
  }
}
