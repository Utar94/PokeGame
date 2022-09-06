namespace PokeGame.Domain.Items.Payloads
{
  public abstract class SaveItemPayload
  {
    public int? Price { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
