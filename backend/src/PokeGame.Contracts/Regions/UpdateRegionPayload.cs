namespace PokeGame.Contracts.Regions;

public record UpdateRegionPayload
{
  public string? Name { get; set; }
  public Change<string>? Description { get; set; }

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }
}
