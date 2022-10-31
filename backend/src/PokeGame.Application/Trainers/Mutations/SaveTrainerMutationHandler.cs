using PokeGame.Domain.Regions;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Application.Trainers.Mutations
{
  internal abstract class SaveTrainerMutationHandler
  {
    protected SaveTrainerMutationHandler(IRepository repository)
    {
      Repository = repository;
    }

    protected IRepository Repository { get; }

    protected async Task EnsureRegionExistsAsync(SaveTrainerPayload payload, CancellationToken cancellationToken)
    {
      if (await Repository.LoadAsync<Region>(payload.RegionId, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Region>(payload.RegionId, nameof(payload.RegionId));
      }
    }
  }
}
