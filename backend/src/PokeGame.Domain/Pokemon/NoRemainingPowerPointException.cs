using PokeGame.Domain.Moves;
using System.Text;

namespace PokeGame.Domain.Pokemon
{
  public class NoRemainingPowerPointException : Exception
  {
    public NoRemainingPowerPointException(Pokemon pokemon, Move move)
      : base(GetMessage(pokemon, move))
    {
      Data["PokemonId"] = pokemon?.Id ?? throw new ArgumentNullException(nameof(pokemon));
      Data["MoveId"] = move?.Id ?? throw new ArgumentNullException(nameof(move));
    }

    private static string GetMessage(Pokemon pokemon, Move move)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified Pokémon move has no remaining power points.");
      message.AppendLine($"Pokémon: {pokemon}");
      message.AppendLine($"Move: {move}");

      return message.ToString();
    }
  }
}
