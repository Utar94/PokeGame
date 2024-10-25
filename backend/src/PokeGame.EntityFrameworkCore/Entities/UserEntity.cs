namespace PokeGame.EntityFrameworkCore.Entities;

internal class UserEntity
{
  public int UserId { get; private set; }

  public Guid Id { get; private set; }
  public string ActorId { get; private set; } = string.Empty;

  public string DisplayName { get; private set; } = string.Empty;
  public string? EmailAddress { get; private set; }
  public string? PictureUrl { get; private set; }

  private UserEntity()
  {
  }
}
