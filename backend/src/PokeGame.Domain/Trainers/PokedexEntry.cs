namespace PokeGame.Domain.Trainers
{
  public class PokedexEntry
  {
    public PokedexEntry(bool hasCaught, DateTime updatedAt)
    {
      HasCaught = hasCaught;
      UpdatedAt = updatedAt;
    }

    public bool HasCaught { get; }
    public DateTime UpdatedAt { get; }
  }
}
