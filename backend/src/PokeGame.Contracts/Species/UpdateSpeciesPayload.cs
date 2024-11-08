﻿namespace PokeGame.Contracts.Species;

public record UpdateSpeciesPayload
{
  public int? Number { get; set; }
  public string? UniqueName { get; set; }
  public Change<string>? DisplayName { get; set; }

  public bool? IsBaby { get; set; }
  public bool? IsLegendary { get; set; }
  public bool? IsMythical { get; set; }

  public int? BaseHappiness { get; set; }
  public int? CaptureRate { get; set; }
  public LevelingRate? LevelingRate { get; set; }

  public Change<string>? Link { get; set; }
  public Change<string>? Notes { get; set; }

  //public List<PokedexNumberModel> PokedexNumbers { get; set; } // TODO(fpion): implement
}