namespace PokeGame.Contracts.Abilities;

public record CreateOrReplaceAbilityPayload
{
  public AbilityKind? Kind { get; set; }

  public string Name { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public CreateOrReplaceAbilityPayload() : this(string.Empty)
  {
  }

  public CreateOrReplaceAbilityPayload(string name)
  {
    Name = name;
  }
}
