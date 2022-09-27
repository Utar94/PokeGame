namespace PokeGame.Domain.Pokemon
{
  public class PokemonEggAlreadyHatchedException : Exception
  {
    public PokemonEggAlreadyHatchedException(Pokemon pokemon)
      : base($"The Pokémon '{pokemon}' has already hatched.")
    {
      Data["PokemonId"] = pokemon?.Id ?? throw new ArgumentNullException(nameof(pokemon));
    }
  }
}
