using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Regions;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Handlers.Regions
{
  internal class SynchronizeRegion
  {
    private readonly ReadContext _readContext;
    private readonly IRepository _repository;

    public SynchronizeRegion(ReadContext readContext, IRepository repository)
    {
      _readContext = readContext;
      _repository = repository;
    }

    public async Task<RegionEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      RegionEntity? entity = await _readContext.Regions
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Region? region = await _repository.LoadAsync<Region>(id, version, cancellationToken);
      if (region != null)
      {
        if (entity == null)
        {
          entity = new RegionEntity { Id = id };
          _readContext.Regions.Add(entity);
        }

        entity.Synchronize(region);

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }

    public async Task<IEnumerable<RegionEntity>> ExecuteAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
      Dictionary<Guid, RegionEntity> entities = await _readContext.Regions
        .Where(x => ids.Contains(x.Id))
        .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

      IEnumerable<Region> regions = await _repository.LoadAsync<Region>(ids, cancellationToken);
      foreach (Region region in regions)
      {
        if (entities.TryGetValue(region.Id, out RegionEntity? entity))
        {
          if (entity.Version >= region.Version)
          {
            continue;
          }
        }
        else
        {
          entity = new RegionEntity { Id = region.Id };
          entities[entity.Id] = entity;
          _readContext.Regions.Add(entity);
        }

        entity.Synchronize(region);
      }

      await _readContext.SaveChangesAsync(cancellationToken);

      return entities.Values;
    }
  }
}
