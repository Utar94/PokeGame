using PokeGame.Core.Models;
using PokeGame.Core.Moves.Models;
using PokeGame.Core.Moves.Payloads;

namespace PokeGame.Core.Moves
{
  public interface IMoveService
  {
    Task<MoveModel> CreateAsync(CreateMovePayload payload, CancellationToken cancellationToken = default);
    Task<MoveModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MoveModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<MoveModel>> GetAsync(string? search = null, PokemonType? type = null,
      MoveSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<MoveModel> UpdateAsync(Guid id, UpdateMovePayload payload, CancellationToken cancellationToken = default);
  }
}
