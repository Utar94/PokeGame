using Logitar.Data;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.PokeGameDb;

internal static class PokemonSpecies
{
  public static TableId Table = new(nameof(PokeGameContext.PokemonSpecies));

  public static ColumnId AggregateId = new(nameof(PokemonSpeciesEntity.AggregateId), Table);
  public static ColumnId CreatedBy = new(nameof(PokemonSpeciesEntity.CreatedBy), Table);
  public static ColumnId CreatedOn = new(nameof(PokemonSpeciesEntity.CreatedOn), Table);
  public static ColumnId UpdatedBy = new(nameof(PokemonSpeciesEntity.UpdatedBy), Table);
  public static ColumnId UpdatedOn = new(nameof(PokemonSpeciesEntity.UpdatedOn), Table);
  public static ColumnId Version = new(nameof(PokemonSpeciesEntity.Version), Table);

  public static ColumnId BaseHappiness = new(nameof(PokemonSpeciesEntity.BaseHappiness), Table);
  public static ColumnId CaptureRate = new(nameof(PokemonSpeciesEntity.CaptureRate), Table);
  public static ColumnId Category = new(nameof(PokemonSpeciesEntity.Category), Table);
  public static ColumnId DisplayName = new(nameof(PokemonSpeciesEntity.DisplayName), Table);
  public static ColumnId Id = new(nameof(PokemonSpeciesEntity.Id), Table);
  public static ColumnId LevelingRate = new(nameof(PokemonSpeciesEntity.LevelingRate), Table);
  public static ColumnId Link = new(nameof(PokemonSpeciesEntity.Link), Table);
  public static ColumnId Notes = new(nameof(PokemonSpeciesEntity.Notes), Table);
  public static ColumnId Number = new(nameof(PokemonSpeciesEntity.Number), Table);
  public static ColumnId PokemonSpeciesId = new(nameof(PokemonSpeciesEntity.PokemonSpeciesId), Table);
  public static ColumnId UniqueName = new(nameof(PokemonSpeciesEntity.UniqueName), Table);
  public static ColumnId UniqueNameNormalized = new(nameof(PokemonSpeciesEntity.UniqueNameNormalized), Table);
}
