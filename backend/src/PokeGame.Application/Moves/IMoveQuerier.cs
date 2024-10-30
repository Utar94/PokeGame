using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves;

public interface IMoveQuerier
{
  Task<MoveModel> ReadAsync(Move move, CancellationToken cancellationToken = default);
  Task<MoveModel?> ReadAsync(MoveId id, CancellationToken cancellationToken = default);
  Task<MoveModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);

  Task<SearchResults<MoveModel>> SearchAsync(SearchMovesPayload payload, CancellationToken cancellationToken = default);
}
