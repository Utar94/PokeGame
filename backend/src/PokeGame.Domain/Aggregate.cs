using System.Reflection;

namespace PokeGame.Domain
{
  public abstract class Aggregate
  {
    private readonly List<DomainEvent> _changes = new();

    public Guid Id { get; private set; } = Guid.NewGuid();
    public int Version { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public Guid CreatedById { get; private set; }

    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedById { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue && DeletedById.HasValue;

    public DateTime? UpdatedAt { get; private set; }
    public Guid? UpdatedById { get; private set; }

    public IReadOnlyCollection<DomainEvent> Changes => _changes.AsReadOnly();
    public bool HasChanges => _changes.Any();

    public static T LoadFromHistory<T>(IEnumerable<DomainEvent> history, Guid id) where T : Aggregate
    {
      ConstructorInfo constructor = typeof(T).GetTypeInfo()
        .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Array.Empty<Type>())
        ?? throw new MissingMethodException(typeof(T).GetName(), "ctor()");

      var aggregate = (T)constructor.Invoke(null)
        ?? throw new InvalidOperationException("The aggregate instance cannot be null.");
      aggregate.Id = id;

      foreach (DomainEvent change in history.OrderBy(x => x.Version))
      {
        aggregate.Dispatch(change);
      }

      return aggregate;
    }

    public void ClearChanges() => _changes.Clear();

    protected void ApplyChange(DomainEvent change)
    {
      change.AggregateId = Id;
      change.OccurredAt = DateTime.UtcNow;
      change.Version = Version + 1;

      Dispatch(change);

      _changes.Add(change);
    }
    private void Dispatch(DomainEvent @event)
    {
      Type aggregateType = GetType();
      Type eventType = @event.GetType();

      MethodInfo method = aggregateType.GetTypeInfo()
        .GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic, new[] { eventType })
        ?? throw new EventNotSupportedException(aggregateType, eventType);

      method.Invoke(this, new[] { @event });

      if (Version == 0)
      {
        CreatedAt = @event.OccurredAt;
        CreatedById = @event.UserId;
      }
      else
      {
        UpdatedAt = @event.OccurredAt;
        UpdatedById = @event.UserId;
      }

      Version = @event.Version;
    }

    protected void Delete(DomainEvent @event)
    {
      DeletedAt = @event.OccurredAt;
      DeletedById = @event.UserId;
    }

    public override bool Equals(object? obj) => obj is Aggregate aggregate
      && aggregate.GetType().Equals(GetType())
      && aggregate.Id == Id;
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
    public override string ToString() => $"{GetType()} (Id={Id})";
  }
}
