using Logitar.Portal.Contracts;

namespace PokeGame.Application.Regions.Models;

public record UpdateRegionPayload
{
  public string? UniqueName { get; set; } = string.Empty;
  public ChangeModel<string>? DisplayName { get; set; }
  public ChangeModel<string>? Description { get; set; }

  public ChangeModel<string>? Link { get; set; }
  public ChangeModel<string>? Notes { get; set; }
}
