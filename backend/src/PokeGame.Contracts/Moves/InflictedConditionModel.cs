namespace PokeGame.Contracts.Moves;

public record InflictedConditionModel
{
  public StatusCondition Condition { get; set; }
  public int Chance { get; set; }
}
