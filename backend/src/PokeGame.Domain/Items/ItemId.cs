using Logitar.EventSourcing;

namespace PokeGame.Domain.Items;

public readonly struct ItemId
{
  public AggregateId AggregateId { get; }
  public string Value => AggregateId.Value;

  public ItemId(Guid id)
  {
    AggregateId = new(id);
  }
  public ItemId(string id)
  {
    AggregateId = new(id);
  }
  public ItemId(AggregateId aggregateId)
  {
    AggregateId = aggregateId;
  }

  public static ItemId NewId() => new(AggregateId.NewId());
  public static ItemId? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value.Trim());

  public Guid ToGuid() => AggregateId.ToGuid();

  public static bool operator ==(ItemId left, ItemId right) => left.Equals(right);
  public static bool operator !=(ItemId left, ItemId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is ItemId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
