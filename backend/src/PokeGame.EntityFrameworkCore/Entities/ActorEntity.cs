using Logitar.Portal.Contracts.Actors;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class ActorEntity
{
  public int ActorId { get; private set; }

  public string Id { get; private set; } = Logitar.EventSourcing.ActorId.DefaultValue;
  public ActorType Type { get; private set; }
  public bool IsDeleted { get; private set; }

  public string DisplayName { get; private set; } = string.Empty;
  public string? EmailAddress { get; private set; }
  public string? PictureUrl { get; private set; }

  private ActorEntity()
  {
  }

  public override bool Equals(object? obj) => obj is ActorEntity actor && actor.Id == Id;
  public override int GetHashCode() => HashCode.Combine(typeof(Actor), Id);
  public override string ToString()
  {
    StringBuilder s = new();

    s.Append(DisplayName);
    if (EmailAddress != null)
    {
      s.Append(" <").Append(EmailAddress).Append('>');
    }
    s.Append(" (").Append(Type).Append(".Id=").Append(Id).Append(')');

    return s.ToString();
  }
}
