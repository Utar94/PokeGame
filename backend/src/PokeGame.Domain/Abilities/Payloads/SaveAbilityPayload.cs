namespace PokeGame.Domain.Abilities.Payloads
{
  public abstract class SaveAbilityPayload
  {
    public AbilityType? Type { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
