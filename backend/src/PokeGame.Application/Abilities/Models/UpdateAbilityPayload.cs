using Logitar.Portal.Contracts;

namespace PokeGame.Application.Abilities.Models;

public record UpdateAbilityPayload
{
  public string? UniqueName { get; set; } = string.Empty;
  public ChangeModel<string>? DisplayName { get; set; }
  public ChangeModel<string>? Description { get; set; }

  public ChangeModel<string>? Link { get; set; }
  public ChangeModel<string>? Notes { get; set; }
}
