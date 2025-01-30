using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves;

public interface IMoveRepository
{
  Task<Move?> LoadAsync(MoveId id, CancellationToken cancellationToken = default);
  Task<Move?> LoadAsync(MoveId id, long? version, CancellationToken cancellationToken = default);

  Task<IReadOnlyCollection<Move>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IReadOnlyCollection<Move>> LoadAsync(IEnumerable<MoveId> ids, CancellationToken cancellationToken = default);

  Task SaveAsync(Move move, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<Move> moves, CancellationToken cancellationToken = default);
}
