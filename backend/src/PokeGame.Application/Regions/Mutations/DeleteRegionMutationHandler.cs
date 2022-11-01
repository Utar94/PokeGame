using MediatR;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Mutations
{
  internal class DeleteRegionMutationHandler : IRequestHandler<DeleteRegionMutation>
  {
    private readonly IRepository _repository;

    public DeleteRegionMutationHandler(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<Unit> Handle(DeleteRegionMutation request, CancellationToken cancellationToken)
    {
      Region region = await _repository.LoadAsync<Region>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Region>(request.Id);

      region.Delete();

      await _repository.SaveAsync(region, cancellationToken);

      return Unit.Value;
    }
  }
}
