using Logitar.EventSourcing;

namespace PokeGame.Domain.Regions;

public readonly struct RegionId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public RegionId(Guid value)
  {
    StreamId = new(value);
  }
  public RegionId(string value)
  {
    StreamId = new(value);
  }
  public RegionId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static RegionId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(RegionId left, RegionId right) => left.Equals(right);
  public static bool operator !=(RegionId left, RegionId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is RegionId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
