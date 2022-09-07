using PokeGame.Application.Moves.Models;

namespace PokeGame.Application.Pokemon.Models
{
  public class PokemonMoveModel
  {
    public MoveModel? Move { get; set; }
    public byte Position { get; set; }
    public byte RemainingPowerPoints { get; set; }
  }
}
