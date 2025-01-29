namespace PokeGame.Application.Abilities.Models;

public record CreateOrReplaceAbilityPayload
{
  public string UniqueName { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }
}
