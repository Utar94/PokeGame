using PokeGame.Core.Models;

namespace PokeGame.Core.Moves.Models
{
  public class MoveModel : AggregateModel
  {
    public PokemonType Type { get; set; }
    public MoveCategory Category { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public byte? Accuracy { get; set; }
    public byte? Power { get; set; }
    public byte PowerPoints { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
