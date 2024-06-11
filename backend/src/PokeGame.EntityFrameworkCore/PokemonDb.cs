using Logitar.Data;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore;

internal static class PokemonDb
{
  public static string Normalize(string value) => value.Trim().ToUpperInvariant();

  public static class Abilities
  {
    public static readonly TableId Table = new(nameof(PokemonContext.Abilities));

    public static readonly ColumnId AbilityId = new(nameof(AbilityEntity.AbilityId), Table);
    public static readonly ColumnId AggregateId = new(nameof(AbilityEntity.AggregateId), Table);
    public static readonly ColumnId CreatedBy = new(nameof(AbilityEntity.CreatedBy), Table);
    public static readonly ColumnId CreatedOn = new(nameof(AbilityEntity.CreatedOn), Table);
    public static readonly ColumnId Description = new(nameof(AbilityEntity.Description), Table);
    public static readonly ColumnId DisplayName = new(nameof(AbilityEntity.DisplayName), Table);
    public static readonly ColumnId Notes = new(nameof(AbilityEntity.Notes), Table);
    public static readonly ColumnId Reference = new(nameof(AbilityEntity.Reference), Table);
    public static readonly ColumnId UniqueName = new(nameof(AbilityEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(AbilityEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UpdatedBy = new(nameof(AbilityEntity.UpdatedBy), Table);
    public static readonly ColumnId UpdatedOn = new(nameof(AbilityEntity.UpdatedOn), Table);
    public static readonly ColumnId Version = new(nameof(AbilityEntity.Version), Table);
  }

  public static class Actors
  {
    public static readonly TableId Table = new(nameof(PokemonContext.Actors));

    public static readonly ColumnId ActorId = new(nameof(ActorEntity.ActorId), Table);
    public static readonly ColumnId DisplayName = new(nameof(ActorEntity.DisplayName), Table);
    public static readonly ColumnId EmailAddress = new(nameof(ActorEntity.EmailAddress), Table);
    public static readonly ColumnId Id = new(nameof(ActorEntity.Id), Table);
    public static readonly ColumnId IsDeleted = new(nameof(ActorEntity.IsDeleted), Table);
    public static readonly ColumnId PictureUrl = new(nameof(ActorEntity.PictureUrl), Table);
    public static readonly ColumnId Type = new(nameof(ActorEntity.Type), Table);
  }
}
