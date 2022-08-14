namespace PokeGame.Core.Models
{
  public abstract class AggregateSummary
  {
    public Guid Id { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
