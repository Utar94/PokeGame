namespace PokeGame.Contracts.Moves;

public record VolatileConditionUpdate
{
  public string VolatileCondition { get; set; }
  public ActionKind Action { get; set; }

  public VolatileConditionUpdate() : this(string.Empty, default)
  {
  }

  public VolatileConditionUpdate(string volatileCondition, ActionKind action)
  {
    VolatileCondition = volatileCondition;
    Action = action;
  }
}
