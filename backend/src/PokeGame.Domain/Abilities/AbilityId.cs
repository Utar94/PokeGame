using Logitar.EventSourcing;

namespace PokeGame.Domain.Abilities;

public readonly struct AbilityId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public AbilityId(Guid value) : this(new AggregateId(value))
  {
  }
  public AbilityId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static AbilityId NewId() => new(AggregateId.NewId());

  public Guid ToGuid() => AggregateId.ToGuid();

  public static bool operator ==(AbilityId left, AbilityId right) => left.Equals(right);
  public static bool operator !=(AbilityId left, AbilityId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is AbilityId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
