namespace PokeGame.Contracts.Species;

public record UpdateSpeciesPayload
{
  public int? Number { get; set; }
  public string? UniqueName { get; set; }
  public Change<string>? DisplayName { get; set; }
  public Change<PokemonCategory?>? Category { get; set; }

  public int? BaseHappiness { get; set; }
  public int? CaptureRate { get; set; }
  public LevelingRate? LevelingRate { get; set; }

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }

  public List<UpdatePokedexNumberPayload> PokedexNumbers { get; set; } = [];
}
