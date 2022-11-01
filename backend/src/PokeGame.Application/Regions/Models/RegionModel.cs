using PokeGame.Application.Models;

namespace PokeGame.Application.Regions.Models
{
  public class RegionModel : AggregateModel
  {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
