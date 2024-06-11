namespace PokeGame.Contracts.Abilities;

public record ReplaceAbilityPayload
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public ReplaceAbilityPayload() : this(string.Empty)
  {
  }

  public ReplaceAbilityPayload(string uniqueName)
  {
    UniqueName = uniqueName;
  }
}
