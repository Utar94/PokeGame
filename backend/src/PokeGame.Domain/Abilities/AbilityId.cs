using Logitar.EventSourcing;
using System.Diagnostics.CodeAnalysis;

namespace PokeGame.Domain.Abilities;

public readonly struct AbilityId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public AbilityId(Guid id)
  {
    AggregateId = new(id);
  }
  public AbilityId(string id)
  {
    AggregateId = new(id);
  }
  public AbilityId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static AbilityId NewId() => new(AggregateId.NewId());
  public static AbilityId? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value.Trim());

  public Guid ToGuid() => AggregateId.ToGuid();

  public static bool operator ==(AbilityId left, AbilityId right) => left.Equals(right);
  public static bool operator !=(AbilityId left, AbilityId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is AbilityId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
