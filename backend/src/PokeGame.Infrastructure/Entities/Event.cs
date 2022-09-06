using PokeGame.Domain;
using System.Text.Json;

namespace PokeGame.Infrastructure.Entities
{
  internal class Event
  {
    private Event()
    {
    }

    public Guid Id { get; private set; }
    public long Sid { get; private set; }

    public DateTime OccurredAt { get; private set; }
    public Guid UserId { get; private set; }
    public int Version { get; private set; }

    public string EventType { get; private set; } = string.Empty;
    public string EventData { get; private set; } = string.Empty;

    public string AggregateType { get; private set; } = string.Empty;
    public Guid AggregateId { get; private set; }

    public static IEnumerable<Event> FromChanges(Aggregate aggregate)
    {
      Type aggregateType = aggregate?.GetType() ?? throw new ArgumentNullException(nameof(aggregate));

      return aggregate.Changes.Select(change =>
      {
        Type eventType = change.GetType();

        return new Event
        {
          OccurredAt = change.OccurredAt,
          UserId = change.UserId,
          Version = change.Version,
          EventType = eventType.GetName(),
          EventData = JsonSerializer.Serialize(change, eventType),
          AggregateType = aggregateType.GetName(),
          AggregateId = aggregate.Id
        };
      });
    }

    public DomainEvent Deserialize()
    {
      Type eventType = Type.GetType(EventType)
        ?? throw new InvalidOperationException($"The type '{EventType}' could not be resolved.");

      return (DomainEvent?)JsonSerializer.Deserialize(EventData, eventType)
        ?? throw new InvalidOperationException($"The event '{Id}' could not be deserialized.");
    }
  }
}
