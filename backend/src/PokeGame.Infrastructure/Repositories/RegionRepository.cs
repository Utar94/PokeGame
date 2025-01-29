﻿using Logitar.EventSourcing;
using PokeGame.Application.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Infrastructure.Repositories;

internal class RegionRepository : Repository, IRegionRepository
{
  public RegionRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Region?> LoadAsync(RegionId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Region?> LoadAsync(RegionId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<Region>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Region>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync<Region>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Region>> LoadAsync(IEnumerable<RegionId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync<Region>(ids.Select(id => id.StreamId), cancellationToken);
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
