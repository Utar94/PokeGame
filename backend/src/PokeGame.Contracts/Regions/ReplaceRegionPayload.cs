namespace PokeGame.Contracts.Regions;

public record ReplaceRegionPayload
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public ReplaceRegionPayload() : this(string.Empty)
  {
  }

  public ReplaceRegionPayload(string uniqueName)
  {
    UniqueName = uniqueName;
  }
}
