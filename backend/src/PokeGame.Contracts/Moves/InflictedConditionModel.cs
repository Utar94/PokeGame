namespace PokeGame.Contracts.Moves;

public record InflictedConditionModel : IInflictedCondition
{
  public StatusCondition Condition { get; set; }
  public int Chance { get; set; }
}
