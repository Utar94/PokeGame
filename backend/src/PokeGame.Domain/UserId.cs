using Logitar.EventSourcing;

namespace PokeGame.Domain;

public readonly struct UserId
{
  public ActorId ActorId { get; }
  public string Value => ActorId.Value;

  public UserId(Guid value) : this(new ActorId(value))
  {
  }
  public UserId(string value) : this(new ActorId(value))
  {
  }
  public UserId(ActorId actorId)
  {
    ActorId = actorId;
  }

  public static UserId NewId() => new(ActorId.NewId());

  public Guid ToGuid() => ActorId.ToGuid();

  public static bool operator ==(UserId left, UserId right) => left.Equals(right);
  public static bool operator !=(UserId left, UserId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is UserId userId && userId.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
