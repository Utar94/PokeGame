using Logitar.EventSourcing;
using PokeGame.Application.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Infrastructure.Repositories;

internal class MoveRepository : Repository, IMoveRepository
{
  public MoveRepository(IEventStore eventStore) : base(eventStore)
  {
  }

  public async Task<Move?> LoadAsync(MoveId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Move?> LoadAsync(MoveId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<Move>(id.StreamId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Move>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync<Move>(cancellationToken);
  }
  public async Task<IReadOnlyCollection<Move>> LoadAsync(IEnumerable<MoveId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync<Move>(ids.Select(id => id.StreamId), cancellationToken);
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
