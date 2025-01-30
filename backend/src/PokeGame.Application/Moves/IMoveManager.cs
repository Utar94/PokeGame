using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves;

public interface IMoveManager
{
  Task SaveAsync(Move move, CancellationToken cancellationToken = default);
}
