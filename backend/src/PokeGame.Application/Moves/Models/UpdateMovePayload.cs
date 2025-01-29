using PokeGame.Domain;

namespace PokeGame.Application.Moves.Models;

public record UpdateMovePayload
{
  public string? UniqueName { get; set; } = string.Empty;
  public Change<string>? DisplayName { get; set; }
  public Change<string>? Description { get; set; }

  public Change<int?>? Accuracy { get; set; }
  public Change<int?>? Power { get; set; }
  public int? PowerPoints { get; set; }

  public Change<InflictedStatusModel>? InflictedStatus { get; set; }
  public List<StatisticChangeModel> StatisticChanges { get; set; } = [];
  public List<string> VolatileConditions { get; set; } = [];

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }
}
