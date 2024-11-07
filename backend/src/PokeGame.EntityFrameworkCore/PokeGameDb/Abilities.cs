using Logitar.Data;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.PokeGameDb;

internal static class Abilities
{
  public static TableId Table = new(nameof(PokeGameContext.Abilities));

  public static ColumnId AggregateId = new(nameof(AbilityEntity.AggregateId), Table);
  public static ColumnId CreatedBy = new(nameof(AbilityEntity.CreatedBy), Table);
  public static ColumnId CreatedOn = new(nameof(AbilityEntity.CreatedOn), Table);
  public static ColumnId UpdatedBy = new(nameof(AbilityEntity.UpdatedBy), Table);
  public static ColumnId UpdatedOn = new(nameof(AbilityEntity.UpdatedOn), Table);
  public static ColumnId Version = new(nameof(AbilityEntity.Version), Table);

  public static ColumnId AbilityId = new(nameof(AbilityEntity.AbilityId), Table);
  public static ColumnId Description = new(nameof(AbilityEntity.Description), Table);
  public static ColumnId DisplayName = new(nameof(AbilityEntity.DisplayName), Table);
  public static ColumnId Id = new(nameof(AbilityEntity.Id), Table);
  public static ColumnId Link = new(nameof(AbilityEntity.Link), Table);
  public static ColumnId Notes = new(nameof(AbilityEntity.Notes), Table);
  public static ColumnId UniqueName = new(nameof(AbilityEntity.UniqueName), Table);
  public static ColumnId UniqueNameNormalized = new(nameof(AbilityEntity.UniqueNameNormalized), Table);
}
