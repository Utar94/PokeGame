using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using PokeGame.Domain.Regions;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class RegionRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IRegionRepository
{
  public RegionRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer)
    : base(eventBus, eventContext, eventSerializer)
  {
  }

  public async Task<IReadOnlyCollection<Region>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<Region>(cancellationToken)).ToArray();
  }
  public async Task<IReadOnlyCollection<Region>> LoadAsync(IEnumerable<RegionId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return (await LoadAsync<Region>(aggregateIds, cancellationToken)).ToArray();
  }

  public async Task<Region?> LoadAsync(RegionId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Region?> LoadAsync(RegionId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<Region>(id.AggregateId, version, cancellationToken);
  }

  public async Task SaveAsync(Region region, CancellationToken cancellationToken)
  {
    await base.SaveAsync(region, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Region> regions, CancellationToken cancellationToken)
  {
    await base.SaveAsync(regions, cancellationToken);
  }
}
