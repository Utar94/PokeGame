using PokeGame.Core.Abilities.Models;
using PokeGame.Core.Abilities.Payloads;
using PokeGame.Core.Models;

namespace PokeGame.Core.Abilities
{
  public interface IAbilityService
  {
    Task<AbilityModel> CreateAsync(CreateAbilityPayload payload, CancellationToken cancellationToken = default);
    Task<AbilityModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AbilityModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<AbilityModel>> GetAsync(string? search = null,
      AbilitySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<AbilityModel> UpdateAsync(Guid id, UpdateAbilityPayload payload, CancellationToken cancellationToken = default);
  }
}
