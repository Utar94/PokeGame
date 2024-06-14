using Logitar.EventSourcing;

namespace PokeGame.Domain.Moves;

public readonly struct MoveId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public MoveId(Guid id)
  {
    AggregateId = new(id);
  }
  public MoveId(string id)
  {
    AggregateId = new(id);
  }
  public MoveId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static MoveId NewId() => new(AggregateId.NewId());
  public static MoveId? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value.Trim());

  public Guid ToGuid() => AggregateId.ToGuid();

  public static bool operator ==(MoveId left, MoveId right) => left.Equals(right);
  public static bool operator !=(MoveId left, MoveId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is MoveId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
