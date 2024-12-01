using Logitar.Portal.Contracts;

namespace PokeGame.Application.Regions.Models;

public class RegionModel : Aggregate
{
  public string UniqueName { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }
}
