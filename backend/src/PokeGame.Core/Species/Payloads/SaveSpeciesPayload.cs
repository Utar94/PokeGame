namespace PokeGame.Core.Species.Payloads
{
  public abstract class SaveSpeciesPayload
  {
    public Guid? AbilityId { get; set; }

    public string Name { get; set; } = null!;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
