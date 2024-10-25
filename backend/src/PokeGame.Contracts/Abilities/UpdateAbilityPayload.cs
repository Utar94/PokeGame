namespace PokeGame.Contracts.Abilities;

public record UpdateAbilityPayload
{
  public Change<AbilityKind?>? Kind { get; set; }

  public string? Name { get; set; }
  public Change<string>? Description { get; set; }

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }
}
