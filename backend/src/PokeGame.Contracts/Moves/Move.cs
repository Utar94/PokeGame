using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Moves;

public class Move : Aggregate
{
  public PokemonType Type { get; set; }
  public MoveCategory Category { get; set; }

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

  public Move() : this(string.Empty)
  {
  }

  public Move(string uniqueName)
  {
    UniqueName = uniqueName;

    StatusConditions = [];
    StatisticChanges = [];
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
