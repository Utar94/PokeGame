using Logitar.EventSourcing;

namespace PokeGame.Domain.Regions;

public readonly struct RegionId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public RegionId(Guid id)
  {
    AggregateId = new(id);
  }
  public RegionId(string id)
  {
    AggregateId = new(id);
  }
  public RegionId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static RegionId NewId() => new(AggregateId.NewId());
  public static RegionId? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value.Trim());

  public Guid ToGuid() => AggregateId.ToGuid();

  public static bool operator ==(RegionId left, RegionId right) => left.Equals(right);
  public static bool operator !=(RegionId left, RegionId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is RegionId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
