using System.Text;

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
