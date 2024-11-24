using Logitar.EventSourcing;

namespace PokeGame.Domain.Species;

public readonly struct SpeciesId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public SpeciesId(Guid value) : this(new AggregateId(value))
  {
  }
  public SpeciesId(string value) : this(new AggregateId(value))
  {
  }
  public SpeciesId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static SpeciesId NewId() => new(AggregateId.NewId());

  public Guid ToGuid() => AggregateId.ToGuid();

  public static bool operator ==(SpeciesId left, SpeciesId right) => left.Equals(right);
  public static bool operator !=(SpeciesId left, SpeciesId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is SpeciesId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
