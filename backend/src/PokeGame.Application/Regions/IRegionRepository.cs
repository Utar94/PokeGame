using Logitar.Identity.Domain.Shared;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions;

public interface IRegionRepository
{
  Task<IReadOnlyCollection<RegionAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<RegionAggregate?> LoadAsync(RegionId id, CancellationToken cancellationToken = default);
  Task<RegionAggregate?> LoadAsync(RegionId id, long? version, CancellationToken cancellationToken = default);
  Task<RegionAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);

  Task SaveAsync(RegionAggregate region, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<RegionAggregate> regions, CancellationToken cancellationToken = default);
}
