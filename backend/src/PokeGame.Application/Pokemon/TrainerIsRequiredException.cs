namespace PokeGame.Application.Pokemon
{
  public class TrainerIsRequiredException : Exception
  {
    public TrainerIsRequiredException(Domain.Pokemon.Pokemon pokemon)
      : base($"A trainer is required for the Pokémon '{pokemon}'.")
    {
      Data["PokemonId"] = pokemon.Id;
    }
  }
}
