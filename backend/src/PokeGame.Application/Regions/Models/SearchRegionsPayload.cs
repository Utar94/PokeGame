using Logitar.Portal.Contracts.Search;

namespace PokeGame.Application.Regions.Models;

public record SearchRegionsPayload : SearchPayload
{
  public new List<RegionSortOption> Sort { get; set; } = [];
}
