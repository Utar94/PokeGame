using Logitar.Data;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.PokeGameDb;

internal static class Species
{
  public static readonly TableId Table = new(nameof(PokeGameContext.Species));

  public static readonly ColumnId CreatedBy = new(nameof(SpeciesEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(SpeciesEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(SpeciesEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(SpeciesEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(SpeciesEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(SpeciesEntity.Version), Table);

  public static readonly ColumnId BaseFriendship = new(nameof(SpeciesEntity.BaseFriendship), Table);
  public static readonly ColumnId CatchRate = new(nameof(SpeciesEntity.CatchRate), Table);
  public static readonly ColumnId Category = new(nameof(SpeciesEntity.Category), Table);
  public static readonly ColumnId DisplayName = new(nameof(SpeciesEntity.DisplayName), Table);
  public static readonly ColumnId GrowthRate = new(nameof(SpeciesEntity.GrowthRate), Table);
  public static readonly ColumnId Id = new(nameof(SpeciesEntity.Id), Table);
  public static readonly ColumnId Link = new(nameof(SpeciesEntity.Link), Table);
  public static readonly ColumnId Notes = new(nameof(SpeciesEntity.Notes), Table);
  public static readonly ColumnId Number = new(nameof(SpeciesEntity.Number), Table);
  public static readonly ColumnId SpeciesId = new(nameof(SpeciesEntity.SpeciesId), Table);
  public static readonly ColumnId UniqueName = new(nameof(SpeciesEntity.UniqueName), Table);
  public static readonly ColumnId UniqueNameNormalized = new(nameof(SpeciesEntity.UniqueNameNormalized), Table);
}
