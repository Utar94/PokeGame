using MediatR;

namespace PokeGame.Application.Species.Mutations
{
  internal class DeleteSpeciesMutationHandler : IRequestHandler<DeleteSpeciesMutation>
  {
    private readonly IRepository _repository;

    public DeleteSpeciesMutationHandler(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<Unit> Handle(DeleteSpeciesMutation request, CancellationToken cancellationToken)
    {
      Domain.Species.Species species = await _repository.LoadAsync<Domain.Species.Species>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(request.Id);

      species.Delete();

      await _repository.SaveAsync(species, cancellationToken);

      return Unit.Value;
    }
  }
}
