namespace PokeGame.Application.Pokemon
{
  public class NoAvailablePositionException : Exception
  {
    public NoAvailablePositionException(Guid trainerId)
      : base($"No Pokémon position is available for the trainer '{trainerId}'.")
    {
      Data["TrainerId"] = trainerId;
    }
  }
}
