using PokeGame.Application.Models;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Moves
{
  public interface IMoveQuerier
  {
    Task<MoveModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<MoveModel>> GetPagedAsync(string? search = null, PokemonType? type = null,
      MoveSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
