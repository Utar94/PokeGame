using PokeGame.Core;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Repositories
{
  internal class Repository<T> : IRepository<T> where T : Aggregate
  {
    private readonly PokeGameDbContext _dbContext;

    public Repository(PokeGameDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task SaveAsync(T aggregate, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(aggregate);

      Save(aggregate);

      await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveAsync(IEnumerable<T> aggregates, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(aggregates);

      foreach (T aggregate in aggregates)
      {
        Save(aggregate);
      }

      await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void Save(T aggregate)
    {
      if (aggregate.HasChanges)
      {
        IEnumerable<Event> events = Event.FromChanges(aggregate);

        _dbContext.Events.AddRange(events);

        aggregate.ClearChanges();
      }

      if (aggregate.IsDeleted)
      {
        _dbContext.Remove(aggregate);
      }
      else if (aggregate.Sid > 0)
      {
        _dbContext.Update(aggregate);
      }
      else
      {
        _dbContext.Add(aggregate);
      }
    }
  }
}
