﻿using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Moves;

public class MoveModel : Aggregate
{
  public PokemonType Type { get; set; }
  public MoveCategory Category { get; set; }

  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
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

  public MoveModel(string uniqueName)
  {
    UniqueName = uniqueName;

    StatisticChanges = [];
    VolatileConditions = [];
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
