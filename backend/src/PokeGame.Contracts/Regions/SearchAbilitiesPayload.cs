using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Regions;

public record SearchAbilitiesPayload : SearchPayload
{
  public new List<RegionSortOption> Sort { get; set; } = [];
}
