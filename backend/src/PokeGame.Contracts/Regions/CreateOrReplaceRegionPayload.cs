namespace PokeGame.Contracts.Regions;

public record CreateOrReplaceRegionPayload
{
  public string Name { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public CreateOrReplaceRegionPayload() : this(string.Empty)
  {
  }

  public CreateOrReplaceRegionPayload(string name)
  {
    Name = name;
  }
}
