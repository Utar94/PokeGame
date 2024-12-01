using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class UserEntity
{
  public int UserId { get; private set; }
  public Guid Id { get; private set; }

  public string ActorId { get; private set; } = string.Empty;
  public bool IsDeleted { get; private set; }

  public string DisplayName { get; private set; } = string.Empty;
  public string? EmailAddress { get; private set; }
  public string? PictureUrl { get; private set; }

  public UserEntity(User user)
  {
    Id = user.Id;

    ActorId = new ActorId(user.Id).Value;

    DisplayName = user.FullName ?? user.UniqueName;
    EmailAddress = user.Email?.Address;
    PictureUrl = user.Picture;
  }

  private UserEntity()
  {
  }
}
