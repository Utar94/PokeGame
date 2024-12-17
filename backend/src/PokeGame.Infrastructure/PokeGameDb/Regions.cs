﻿using Logitar.Data;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.PokeGameDb;

internal static class Regions
{
  public static readonly TableId Table = new(nameof(PokeGameContext.Regions));

  public static readonly ColumnId CreatedBy = new(nameof(RegionEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(RegionEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(RegionEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(RegionEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(RegionEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(RegionEntity.Version), Table);

  public static readonly ColumnId Description = new(nameof(RegionEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(RegionEntity.DisplayName), Table);
  public static readonly ColumnId Id = new(nameof(RegionEntity.Id), Table);
  public static readonly ColumnId Link = new(nameof(RegionEntity.Link), Table);
  public static readonly ColumnId Notes = new(nameof(RegionEntity.Notes), Table);
  public static readonly ColumnId RegionId = new(nameof(RegionEntity.RegionId), Table);
  public static readonly ColumnId UniqueName = new(nameof(RegionEntity.UniqueName), Table);
  public static readonly ColumnId UniqueNameNormalized = new(nameof(RegionEntity.UniqueNameNormalized), Table);
}
