using Logitar.EventSourcing;

namespace PokeGame.Domain.Speciez;

public readonly struct SpeciesId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public SpeciesId(string value)
  {
    StreamId = new(value);
  }
  public SpeciesId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static SpeciesId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(SpeciesId left, SpeciesId right) => left.Equals(right);
  public static bool operator !=(SpeciesId left, SpeciesId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is SpeciesId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
