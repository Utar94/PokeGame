using PokeGame.Domain.Moves;
using System.Text;

namespace PokeGame.Domain.Pokemon
{
  public class PokemonMoveNotFoundException : Exception
  {
    public PokemonMoveNotFoundException(Pokemon pokemon, Move move)
      : base(GetMessage(pokemon, move))
    {
      Data["PokemonId"] = pokemon.Id;
      Data["MoveId"] = move.Id;
    }

    private static string GetMessage(Pokemon pokemon, Move move)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified Pokémon move could not be found.");
      message.AppendLine($"Pokémon: {pokemon}");
      message.AppendLine($"Move: {move}");

      return message.ToString();
    }
  }
}
