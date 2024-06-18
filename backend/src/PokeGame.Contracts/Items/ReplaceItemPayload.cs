namespace PokeGame.Contracts.Items;

public record ReplaceItemPayload
{
  public int? Price { get; set; }

  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
  public string? Picture { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public ReplaceItemPayload() : this(string.Empty)
  {
  }

  public ReplaceItemPayload(string uniqueName)
  {
    UniqueName = uniqueName;
  }
}
