namespace PokeGame.Domain.Pokemon
{
  public class CannotWoundFaintedPokemonException : Exception
  {
    public CannotWoundFaintedPokemonException(Pokemon pokemon)
      : base($"The fainted Pokémon '{pokemon}' cannot be wounded.")
    {
      Data["PokemonId"] = pokemon?.Id ?? throw new ArgumentNullException(nameof(pokemon));
    }
  }
}
