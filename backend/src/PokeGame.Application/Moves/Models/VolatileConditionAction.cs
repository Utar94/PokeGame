namespace PokeGame.Application.Moves.Models;

public record VolatileConditionAction
{
  public string Value { get; set; } = string.Empty;
  public CollectionAction Action { get; set; }

  public VolatileConditionAction()
  {
  }

  public VolatileConditionAction(string value, CollectionAction action = CollectionAction.Add)
  {
    Value = value;
    Action = action;
  }
}
