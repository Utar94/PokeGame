using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;

namespace PokeGame.Application.Abilities
{
  public interface IAbilityQuerier
  {
    Task<AbilityModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<AbilityModel>> GetPagedAsync(string? search = null,
      AbilitySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
