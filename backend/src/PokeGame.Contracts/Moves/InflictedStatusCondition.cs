namespace PokeGame.Contracts.Moves;

public record InflictedStatusCondition
{
  public string StatusCondition { get; set; }
  public int Chance { get; set; }

  public InflictedStatusCondition() : this(string.Empty)
  {
  }

  public InflictedStatusCondition(KeyValuePair<string, int> statusCondition) : this(statusCondition.Key, statusCondition.Value)
  {
  }

  public InflictedStatusCondition(string statusCondition, int chance = 0)
  {
    StatusCondition = statusCondition;
    Chance = chance;
  }
}
