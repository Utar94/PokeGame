using PokeGame.Contracts.Regions;

namespace PokeGame.Contracts.Species;

public record PokedexNumberModel
{
  public RegionModel Region { get; set; }
  public int Number { get; set; }

  public PokedexNumberModel() : this(new RegionModel(), number: 0)
  {
  }

  public PokedexNumberModel(RegionModel region, int number)
  {
    Region = region;
    Number = number;
  }
}
