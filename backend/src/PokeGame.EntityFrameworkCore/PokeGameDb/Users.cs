using Logitar.Data;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.PokeGameDb;

internal static class Users
{
  public static TableId Table = new(nameof(PokeGameContext.Users));

  public static ColumnId DisplayName = new(nameof(UserEntity.DisplayName), Table);
  public static ColumnId EmailAddress = new(nameof(UserEntity.EmailAddress), Table);
  public static ColumnId Id = new(nameof(UserEntity.Id), Table);
  public static ColumnId PictureUrl = new(nameof(UserEntity.PictureUrl), Table);
  public static ColumnId UserId = new(nameof(UserEntity.UserId), Table);
  public static ColumnId v = new(nameof(UserEntity.ActorId), Table);
}
