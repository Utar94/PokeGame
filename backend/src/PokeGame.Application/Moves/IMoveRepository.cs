using Logitar.Identity.Domain.Shared;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves;

public interface IMoveRepository
{
  Task<IReadOnlyCollection<MoveAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<MoveAggregate?> LoadAsync(MoveId id, CancellationToken cancellationToken = default);
  Task<MoveAggregate?> LoadAsync(MoveId id, long? version, CancellationToken cancellationToken = default);
  Task<MoveAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken = default);

  Task SaveAsync(MoveAggregate move, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<MoveAggregate> moves, CancellationToken cancellationToken = default);
}
