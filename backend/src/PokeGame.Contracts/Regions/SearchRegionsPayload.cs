using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Regions;

public record SearchRegionsPayload : SearchPayload
{
  public new List<RegionSortOption> Sort { get; set; } = [];
}
