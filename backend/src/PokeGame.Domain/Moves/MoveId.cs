using Logitar.EventSourcing;

namespace PokeGame.Domain.Moves;

public readonly struct MoveId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public MoveId(Guid value)
  {
    StreamId = new(value);
  }
  public MoveId(string value)
  {
    StreamId = new(value);
  }
  public MoveId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static MoveId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(MoveId left, MoveId right) => left.Equals(right);
  public static bool operator !=(MoveId left, MoveId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is MoveId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
