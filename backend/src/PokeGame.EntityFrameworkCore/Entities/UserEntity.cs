using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Users;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class UserEntity
{
  public int UserId { get; private set; }

  public Guid Id { get; private set; }
  public string ActorId { get; private set; } = string.Empty;

  public string DisplayName { get; private set; } = string.Empty;
  public string? EmailAddress { get; private set; }
  public string? PictureUrl { get; private set; }

  public UserEntity(User user)
  {
    Id = user.Id;
    ActorId = new ActorId(user.Id).Value;

    Update(user);
  }

  private UserEntity()
  {
  }

  public void Update(User user)
  {
    DisplayName = user.FullName ?? user.UniqueName;
    EmailAddress = user.Email?.Address;
    PictureUrl = user.Picture;
  }

  public override bool Equals(object? obj) => obj is UserEntity user && user.Id == Id;
  public override int GetHashCode() => Id.GetHashCode();
  public override string ToString()
  {
    StringBuilder s = new();
    s.Append(DisplayName);
    if (EmailAddress != null)
    {
      s.Append(" <").Append(EmailAddress).Append('>');
    }
    s.Append(" (User.Id=").Append(Id).Append(')');
    return s.ToString();
  }
}
