using Logitar.Portal.Contracts;

namespace PokeGame.Application.Moves.Models;

public record UpdateMovePayload
{
  public string? UniqueName { get; set; } = string.Empty;
  public ChangeModel<string>? DisplayName { get; set; }
  public ChangeModel<string>? Description { get; set; }

  public ChangeModel<int?>? Accuracy { get; set; }
  public ChangeModel<int?>? Power { get; set; }
  public int? PowerPoints { get; set; }

  public ChangeModel<InflictedStatusModel>? InflictedStatus { get; set; }
  public List<StatisticChangeModel> StatisticChanges { get; set; } = [];
  public List<VolatileConditionAction> VolatileConditions { get; set; } = [];

  public ChangeModel<string>? Link { get; set; }
  public ChangeModel<string>? Notes { get; set; }
}
