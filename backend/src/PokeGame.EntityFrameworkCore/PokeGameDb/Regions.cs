using Logitar.Data;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.PokeGameDb;

internal static class Regions
{
  public static TableId Table = new(nameof(PokeGameContext.Regions));

  public static ColumnId AggregateId = new(nameof(RegionEntity.AggregateId), Table);
  public static ColumnId CreatedBy = new(nameof(RegionEntity.CreatedBy), Table);
  public static ColumnId CreatedOn = new(nameof(RegionEntity.CreatedOn), Table);
  public static ColumnId UpdatedBy = new(nameof(RegionEntity.UpdatedBy), Table);
  public static ColumnId UpdatedOn = new(nameof(RegionEntity.UpdatedOn), Table);
  public static ColumnId Version = new(nameof(RegionEntity.Version), Table);

  public static ColumnId Description = new(nameof(RegionEntity.Description), Table);
  public static ColumnId Id = new(nameof(RegionEntity.Id), Table);
  public static ColumnId Link = new(nameof(RegionEntity.Link), Table);
  public static ColumnId Name = new(nameof(RegionEntity.Name), Table);
  public static ColumnId Notes = new(nameof(RegionEntity.Notes), Table);
  public static ColumnId RegionId = new(nameof(RegionEntity.RegionId), Table);
}
