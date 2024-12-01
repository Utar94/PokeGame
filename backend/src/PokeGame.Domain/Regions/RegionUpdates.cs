namespace PokeGame.Domain.Regions;

public record RegionUpdates
{
  public UniqueName? UniqueName { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<Url>? Link { get; set; }
  public Change<Notes>? Notes { get; set; }
}
