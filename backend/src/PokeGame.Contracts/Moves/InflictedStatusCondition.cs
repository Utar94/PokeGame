namespace PokeGame.Contracts.Moves;

public record InflictedStatusCondition
{
  public string StatusCondition { get; set; }
  public int Chance { get; set; }

  public InflictedStatusCondition() : this(string.Empty)
  {
  }

  public InflictedStatusCondition(string statusCondition)
  {
    StatusCondition = statusCondition;
  }
}
