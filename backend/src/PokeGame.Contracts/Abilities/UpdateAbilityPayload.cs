namespace PokeGame.Contracts.Abilities;

public record UpdateAbilityPayload
{
  public string? UniqueName { get; set; }
  public Change<string>? DisplayName { get; set; }
  public Change<string>? Description { get; set; }

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }
}
