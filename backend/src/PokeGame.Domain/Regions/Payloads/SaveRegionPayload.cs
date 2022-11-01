namespace PokeGame.Domain.Regions.Payloads
{
  public abstract class SaveRegionPayload
  {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
