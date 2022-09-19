using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure
{
  internal class Repository<T> : IRepository<T> where T : Aggregate
  {
    private readonly EventContext _eventContext;
    private readonly IPublisher _publisher;
    private readonly IUserContext _userContext;

    public Repository(EventContext eventContext, IPublisher publisher, IUserContext userContext)
    {
      _eventContext = eventContext;
      _publisher = publisher;
      _userContext = userContext;
    }

    public async Task<T?> LoadAsync(Guid id, CancellationToken cancellationToken)
      => await LoadAsync(id, version: null, cancellationToken);
    public async Task<T?> LoadAsync(Guid id, int? version, CancellationToken cancellationToken)
    {
      string aggregateType = typeof(T).GetName();

      IQueryable<Event> query = _eventContext.Events.AsNoTracking()
        .Where(x => x.AggregateType == aggregateType && x.AggregateId == id);

      if (version.HasValue)
      {
        query = query.Where(x => x.Version <= version.Value);
      }

      Event[] events = await query.OrderBy(x => x.Version).ToArrayAsync(cancellationToken);

      return Load(events, id);
    }

    public async Task<IEnumerable<T>> LoadAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
      string aggregateType = typeof(T).GetName();

      IEnumerable<IGrouping<Guid, Event>> events = (await _eventContext.Events.AsNoTracking()
        .Where(x => x.AggregateType == aggregateType && ids.Contains(x.AggregateId))
        .OrderBy(x => x.Version)
        .ToArrayAsync(cancellationToken)
      ).GroupBy(x => x.AggregateId);

      var aggregates = new List<T>(capacity: events.Count());
      foreach (IGrouping<Guid, Event> group in events)
      {
        T? aggregate = Load(group, group.Key);
        if (aggregate != null)
        {
          aggregates.Add(aggregate);
        }
      }

      return aggregates;
    }

    public async Task SaveAsync(T instance, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(instance);

      if (instance.HasChanges)
      {
        foreach (DomainEvent change in instance.Changes)
        {
          if (change.UserId == Guid.Empty)
          {
            change.UserId = _userContext.Id;
          }
        }

        IEnumerable<Event> events = Event.FromChanges(instance);
        _eventContext.AddRange(events);
        await _eventContext.SaveChangesAsync(cancellationToken);

        foreach (DomainEvent change in instance.Changes)
        {
          await _publisher.Publish(change, cancellationToken);
        }
      }
    }

    public async Task SaveAsync(IEnumerable<T> instances, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(instances);

      var changes = new List<DomainEvent>();
      var entities = new List<Event>();

      foreach (T instance in instances)
      {
        if (instance.HasChanges)
        {
          foreach (DomainEvent change in instance.Changes)
          {
            if (change.UserId == Guid.Empty)
            {
              change.UserId = _userContext.Id;
            }
          }

          changes.AddRange(instance.Changes);
          entities.AddRange(Event.FromChanges(instance));
        }
      }

      if (entities.Any())
      {
        _eventContext.Events.AddRange(entities);
        await _eventContext.SaveChangesAsync(cancellationToken);
      }

      if (changes.Any())
      {
        foreach (DomainEvent change in changes)
        {
          await _publisher.Publish(change, cancellationToken);
        }
      }
    }

    private static T? Load(IEnumerable<Event> events, Guid id)
    {
      if (!events.Any())
      {
        return null;
      }

      IEnumerable<DomainEvent> history = events.Select(e => e.Deserialize());
      var aggregate = Aggregate.LoadFromHistory<T>(history, id);
      if (aggregate.IsDeleted)
      {
        return null;
      }

      return aggregate;
    }
  }
}
