using Logitar.Data;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.PokeGameDb;

internal static class RegionalSpecies
{
  public static readonly TableId Table = new(nameof(PokeGameContext.RegionalSpecies));

  public static readonly ColumnId Number = new(nameof(RegionalSpeciesEntity.Number), Table);
  public static readonly ColumnId RegionId = new(nameof(RegionalSpeciesEntity.RegionId), Table);
  public static readonly ColumnId SpeciesId = new(nameof(RegionalSpeciesEntity.SpeciesId), Table);
}
