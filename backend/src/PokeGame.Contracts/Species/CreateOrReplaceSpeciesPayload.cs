﻿namespace PokeGame.Contracts.Species;

public record CreateOrReplaceSpeciesPayload
{
  public int Number { get; set; }
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public PokemonCategory? Category { get; set; }

  public int BaseHappiness { get; set; }
  public int CaptureRate { get; set; }
  public LevelingRate LevelingRate { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public List<PokedexNumberPayload> PokedexNumbers { get; set; }

  public CreateOrReplaceSpeciesPayload() : this(number: 0, string.Empty)
  {
  }

  public CreateOrReplaceSpeciesPayload(int number, string uniqueName)
  {
    Number = number;
    UniqueName = uniqueName;

    PokedexNumbers = [];
  }
}
