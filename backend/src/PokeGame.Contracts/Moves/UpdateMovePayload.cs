namespace PokeGame.Contracts.Moves;

public record UpdateMovePayload
{
  public Change<MoveKind?>? Kind { get; set; }

  public string? Name { get; set; }
  public Change<string>? Description { get; set; }

  public Change<int?>? Accuracy { get; set; }
  public Change<int?>? Power { get; set; }
  public int? PowerPoints { get; set; }

  //public List<StatisticChangeModel> StatisticChanges { get; set; } // TODO(fpion): implement
  public Change<InflictedConditionModel>? Status { get; set; }
  //public List<string> VolatileConditions { get; set; } // TODO(fpion): implement

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }
}
