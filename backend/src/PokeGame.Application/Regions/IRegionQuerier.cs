using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions;

public interface IRegionQuerier
{
  Task<Region> ReadAsync(RegionAggregate region, CancellationToken cancellationToken = default);
  Task<Region?> ReadAsync(RegionId id, CancellationToken cancellationToken = default);
  Task<Region?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<Region?> ReadAsync(string uniqueName, CancellationToken cancellationToken = default);

  Task<SearchResults<Region>> SearchAsync(SearchRegionsPayload payload, CancellationToken cancellationToken = default);
}
