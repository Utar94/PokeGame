using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Speciez.Models;

public record RegionalNumberModel
{
  public RegionModel Region { get; set; } = new();
  public int Number { get; set; }
}
