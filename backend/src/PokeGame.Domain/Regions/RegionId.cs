using Logitar.EventSourcing;

namespace PokeGame.Domain.Regions;

public readonly struct RegionId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public RegionId(Guid value) : this(new AggregateId(value))
  {
  }
  public RegionId(string value) : this(new AggregateId(value))
  {
  }
  public RegionId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static RegionId NewId() => new(AggregateId.NewId());

  public Guid ToGuid() => AggregateId.ToGuid();

  public static bool operator ==(RegionId left, RegionId right) => left.Equals(right);
  public static bool operator !=(RegionId left, RegionId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is RegionId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
