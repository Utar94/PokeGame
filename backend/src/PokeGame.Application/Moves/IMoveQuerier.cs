using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves;

public interface IMoveQuerier
{
  Task<Move> ReadAsync(MoveAggregate ability, CancellationToken cancellationToken = default);
  Task<Move?> ReadAsync(MoveId id, CancellationToken cancellationToken = default);
  Task<Move?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<Move?> ReadAsync(string uniqueName, CancellationToken cancellationToken = default);

  Task<SearchResults<Move>> SearchAsync(SearchMovesPayload payload, CancellationToken cancellationToken = default);
}
