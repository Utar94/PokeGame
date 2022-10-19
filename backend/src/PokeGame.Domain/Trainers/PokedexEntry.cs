namespace PokeGame.Domain.Trainers
{
  public class PokedexEntry
  {
    public PokedexEntry(bool hasCaught, DateTime updatedOn)
    {
      HasCaught = hasCaught;
      UpdatedOn = updatedOn;
    }

    public bool HasCaught { get; }
    public DateTime UpdatedOn { get; }
  }
}
