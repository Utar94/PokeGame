namespace PokeGame.Contracts.Regions;

public record CreateRegionPayload
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public CreateRegionPayload() : this(string.Empty)
  {
  }

  public CreateRegionPayload(string uniqueName)
  {
    UniqueName = uniqueName;
  }
}
