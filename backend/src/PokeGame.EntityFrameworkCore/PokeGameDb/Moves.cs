using Logitar.Data;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.PokeGameDb;

internal static class Moves
{
  public static TableId Table = new(nameof(PokeGameContext.Moves));

  public static ColumnId AggregateId = new(nameof(MoveEntity.AggregateId), Table);
  public static ColumnId CreatedBy = new(nameof(MoveEntity.CreatedBy), Table);
  public static ColumnId CreatedOn = new(nameof(MoveEntity.CreatedOn), Table);
  public static ColumnId UpdatedBy = new(nameof(MoveEntity.UpdatedBy), Table);
  public static ColumnId UpdatedOn = new(nameof(MoveEntity.UpdatedOn), Table);
  public static ColumnId Version = new(nameof(MoveEntity.Version), Table);

  public static ColumnId Accuracy = new(nameof(MoveEntity.Accuracy), Table);
  public static ColumnId AccuracyStages = new(nameof(MoveEntity.AccuracyStages), Table);
  public static ColumnId AttackStages = new(nameof(MoveEntity.AttackStages), Table);
  public static ColumnId Category = new(nameof(MoveEntity.Category), Table);
  public static ColumnId DefenseStages = new(nameof(MoveEntity.DefenseStages), Table);
  public static ColumnId Description = new(nameof(MoveEntity.Description), Table);
  public static ColumnId EvasionStages = new(nameof(MoveEntity.EvasionStages), Table);
  public static ColumnId Id = new(nameof(MoveEntity.Id), Table);
  public static ColumnId InflictedStatusChance = new(nameof(MoveEntity.InflictedStatusChance), Table);
  public static ColumnId InflictedStatusCondition = new(nameof(MoveEntity.InflictedStatusCondition), Table);
  public static ColumnId Kind = new(nameof(MoveEntity.Kind), Table);
  public static ColumnId Link = new(nameof(MoveEntity.Link), Table);
  public static ColumnId MoveId = new(nameof(MoveEntity.MoveId), Table);
  public static ColumnId Name = new(nameof(MoveEntity.Name), Table);
  public static ColumnId Notes = new(nameof(MoveEntity.Notes), Table);
  public static ColumnId Power = new(nameof(MoveEntity.Power), Table);
  public static ColumnId PowerPoints = new(nameof(MoveEntity.PowerPoints), Table);
  public static ColumnId SpecialAttackStages = new(nameof(MoveEntity.SpecialAttackStages), Table);
  public static ColumnId SpecialDefenseStages = new(nameof(MoveEntity.SpecialDefenseStages), Table);
  public static ColumnId SpeedStages = new(nameof(MoveEntity.SpeedStages), Table);
  public static ColumnId Type = new(nameof(MoveEntity.Type), Table);
  public static ColumnId VolatileConditions = new(nameof(MoveEntity.VolatileConditions), Table);
}
