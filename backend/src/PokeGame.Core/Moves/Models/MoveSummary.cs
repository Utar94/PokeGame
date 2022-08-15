using PokeGame.Core.Models;

namespace PokeGame.Core.Moves.Models
{
  public class MoveSummary : AggregateSummary
  {
    public PokemonType Type { get; set; }
    public Category Category { get; set; }

    public string Name { get; set; } = null!;
  }
}
