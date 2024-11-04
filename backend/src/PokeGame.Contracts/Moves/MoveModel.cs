using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Moves;

public class MoveModel : Aggregate
{
  public PokemonType Type { get; set; }
  public MoveCategory Category { get; set; }
  public MoveKind? Kind { get; set; }

  public string Name { get; set; }
  public string? Description { get; set; }

  public int? Accuracy { get; set; }
  public int? Power { get; set; }
  public int PowerPoints { get; set; }

  public List<StatisticChangeModel> StatisticChanges { get; set; }
  public InflictedConditionModel? Status { get; set; }
  public List<string> VolatileConditions { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public MoveModel() : this(string.Empty)
  {
  }

  public MoveModel(string name)
  {
    Name = name;

    StatisticChanges = [];
    VolatileConditions = [];
  }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
