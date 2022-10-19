using System.Text;

namespace PokeGame.Application.Pokemon
{
  public class CannotSwapPokemonException : Exception
  {
    public CannotSwapPokemonException(Domain.Pokemon.Pokemon pokemon, Domain.Pokemon.Pokemon other)
      : base(GetMessage(pokemon, other))
    {
      Data["PokemonId"] = pokemon.Id;
      Data["OtherPokemonId"] = other.Id;
    }

    private static string GetMessage(Domain.Pokemon.Pokemon pokemon, Domain.Pokemon.Pokemon other)
    {
      var message = new StringBuilder();

      message.AppendLine("The two specified Pokémon cannot be swaped.");
      message.AppendLine($"Pokémon: {pokemon}");
      message.AppendLine($"Other Pokémon: {other}");

      return message.ToString();
    }
  }
}
