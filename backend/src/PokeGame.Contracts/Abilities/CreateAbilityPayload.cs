namespace PokeGame.Contracts.Abilities;

public record CreateAbilityPayload
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public CreateAbilityPayload() : this(string.Empty)
  {
  }

  public CreateAbilityPayload(string uniqueName)
  {
    UniqueName = uniqueName;
  }
}
