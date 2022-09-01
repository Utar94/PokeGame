using PokeGame.Core.Models;

namespace PokeGame.Core.Species.Models
{
  public class SpeciesSummary : AggregateSummary
  {
    public int Number { get; set; }

    public PokemonType PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public string Name { get; set; } = null!;
    public string? Category { get; set; }
  }
}
