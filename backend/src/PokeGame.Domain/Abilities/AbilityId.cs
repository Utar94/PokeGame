using Logitar.EventSourcing;

namespace PokeGame.Domain.Abilities;

public readonly struct AbilityId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public AbilityId(string value)
  {
    StreamId = new(value);
  }
  public AbilityId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static AbilityId NewId() => new(StreamId.NewId());

  public static bool operator ==(AbilityId left, AbilityId right) => left.Equals(right);
  public static bool operator !=(AbilityId left, AbilityId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is AbilityId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
