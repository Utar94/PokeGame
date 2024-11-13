using Logitar.Portal.Contracts.Search;

namespace PokeGame.Contracts.Species;

public record SearchSpeciesPayload : SearchPayload
{
  public PokemonCategory? Category { get; set; }
  public LevelingRate? LevelingRate { get; set; }
  public Guid? RegionId { get; set; }

  public new List<SpeciesSortOption> Sort { get; set; } = [];
}
