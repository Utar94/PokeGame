using PokeGame.Application.Models;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves.Payloads;

namespace PokeGame.Application.Moves
{
  public interface IMoveService
  {
    Task<MoveModel> CreateAsync(CreateMovePayload payload, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MoveModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<MoveModel>> GetAsync(string? search = null, PokemonType? type = null,
      MoveSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<MoveModel> UpdateAsync(Guid id, UpdateMovePayload payload, CancellationToken cancellationToken = default);
  }
}
