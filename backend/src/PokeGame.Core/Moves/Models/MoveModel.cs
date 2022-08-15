using PokeGame.Core.Models;

namespace PokeGame.Core.Moves.Models
{
  public class MoveModel : AggregateModel
  {
    public PokemonType Type { get; set; }
    public Category Category { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public double? Accuracy { get; set; }
    public int? Power { get; set; }
    public int PowerPoints { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
