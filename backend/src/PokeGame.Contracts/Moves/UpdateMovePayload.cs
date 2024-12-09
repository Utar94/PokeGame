﻿namespace PokeGame.Contracts.Moves;

public record UpdateMovePayload
{
  public string? UniqueName { get; set; }
  public Change<string>? DisplayName { get; set; }
  public Change<string>? Description { get; set; }

  public Change<int?>? Accuracy { get; set; }
  public Change<int?>? Power { get; set; }
  public int? PowerPoints { get; set; }

  public List<StatisticChangeModel> StatisticChanges { get; set; } = [];
  public Change<InflictedConditionModel>? Status { get; set; }
  public List<VolatileConditionUpdate> VolatileConditions { get; set; } = [];

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }
}
