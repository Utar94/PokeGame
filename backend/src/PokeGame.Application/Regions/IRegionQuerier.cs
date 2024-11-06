using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions;

public interface IRegionQuerier
{
  Task<RegionModel> ReadAsync(Region region, CancellationToken cancellationToken = default);
  Task<RegionModel?> ReadAsync(RegionId id, CancellationToken cancellationToken = default);
  Task<RegionModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<RegionModel?> ReadAsync(string uniqueName, CancellationToken cancellationToken = default);

  Task<SearchResults<RegionModel>> SearchAsync(SearchRegionsPayload payload, CancellationToken cancellationToken = default);
}
