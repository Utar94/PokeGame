namespace PokeGame.Core.Abilities.Payloads
{
  public abstract class SaveAbilityPayload
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
