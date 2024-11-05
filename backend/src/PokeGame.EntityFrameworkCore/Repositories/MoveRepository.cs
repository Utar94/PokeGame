using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using PokeGame.Domain.Moves;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class MoveRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IMoveRepository
{
  public MoveRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer)
    : base(eventBus, eventContext, eventSerializer)
  {
  }

  public async Task<Move?> LoadAsync(MoveId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Move?> LoadAsync(MoveId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<Move>(id.AggregateId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Move>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<Move>(cancellationToken)).ToArray().AsReadOnly();
  }
  public async Task<IReadOnlyCollection<Move>> LoadAsync(IEnumerable<MoveId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return (await LoadAsync<Move>(aggregateIds, cancellationToken)).ToArray().AsReadOnly();
  }

  public async Task SaveAsync(Move move, CancellationToken cancellationToken)
  {
    await base.SaveAsync(move, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Move> moves, CancellationToken cancellationToken)
  {
    await base.SaveAsync(moves, cancellationToken);
  }
}
