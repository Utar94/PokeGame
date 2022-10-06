namespace PokeGame.Domain.Pokemon
{
  public class CannotCatchTrainerPokemonException : Exception
  {
    public CannotCatchTrainerPokemonException(Pokemon pokemon)
      : base($"The trainer Pokémon '{pokemon}' cannot be caught.")
    {
      Data["PokemonId"] = pokemon.Id;
    }
  }
}
