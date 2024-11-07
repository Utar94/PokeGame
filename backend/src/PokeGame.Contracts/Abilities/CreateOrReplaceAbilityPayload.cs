namespace PokeGame.Contracts.Abilities;

public record CreateOrReplaceAbilityPayload
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public CreateOrReplaceAbilityPayload() : this(string.Empty)
  {
  }

  public CreateOrReplaceAbilityPayload(string uniqueName)
  {
    UniqueName = uniqueName;
  }
}
