﻿using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Species;

public class SpeciesModel : Aggregate
{
  public int Number { get; set; }
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }

  public bool IsBaby { get; set; }
  public bool IsLegendary { get; set; }
  public bool IsMythical { get; set; }

  public int BaseHappiness { get; set; }
  public int CaptureRate { get; set; }
  public LevelingRate LevelingRate { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public List<PokedexNumberModel> PokedexNumbers { get; set; }

  public SpeciesModel() : this(number: 0, string.Empty)
  {
  }

  public SpeciesModel(int number, string uniqueName)
  {
    Number = number;
    UniqueName = uniqueName;

    PokedexNumbers = [];
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} (#{Number}) | {base.ToString()}";
}