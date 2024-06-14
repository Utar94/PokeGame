namespace PokeGame.Contracts.Moves;

public record ReplaceMovePayload
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public int? Accuracy { get; set; }
  public int? Power { get; set; }
  public int PowerPoints { get; set; }

  public List<StatisticChange> StatisticChanges { get; set; }
  public List<InflictedStatusCondition> StatusConditions { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public ReplaceMovePayload() : this(string.Empty)
  {
  }

  public ReplaceMovePayload(string uniqueName)
  {
    UniqueName = uniqueName;

    StatusConditions = [];
    StatisticChanges = [];
  }
}
