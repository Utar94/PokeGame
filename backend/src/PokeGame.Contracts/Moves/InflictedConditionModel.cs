namespace PokeGame.Contracts.Moves;

public record InflictedConditionModel : IInflictedCondition
{
  public StatusCondition Condition { get; set; }
  public int Chance { get; set; }

  public InflictedConditionModel() : this(default, default)
  {
  }

  public InflictedConditionModel(IInflictedCondition status) : this(status.Condition, status.Chance)
  {
  }

  public InflictedConditionModel(StatusCondition condition, int chance)
  {
    Condition = condition;
    Chance = chance;
  }
}
