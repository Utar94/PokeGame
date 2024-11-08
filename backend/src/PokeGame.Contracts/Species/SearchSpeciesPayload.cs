using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Species;

public record SearchSpeciesPayload : SearchPayload
{
  public bool? IsBaby { get; set; }
  public bool? IsLegendary { get; set; }
  public bool? IsMythical { get; set; }

  public LevelingRate? LevelingRate { get; set; }

  public Guid? RegionId { get; set; }

  public new List<SpeciesSortOption> Sort { get; set; } = [];
}
