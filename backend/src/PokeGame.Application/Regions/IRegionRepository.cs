using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions;

public interface IRegionRepository
{
  Task<Region?> LoadAsync(RegionId id, CancellationToken cancellationToken = default);
  Task<Region?> LoadAsync(RegionId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Region>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Region>> LoadAsync(IEnumerable<RegionId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Region region, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Region> regions, CancellationToken cancellationToken = default);
}
