using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Abilities.Payloads;

namespace PokeGame.Application.Abilities
{
  public interface IAbilityService
  {
    Task<AbilityModel> CreateAsync(CreateAbilityPayload payload, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AbilityModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<AbilityModel>> GetAsync(string? search = null, Guid? speciesId = null,
      AbilitySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<AbilityModel> UpdateAsync(Guid id, UpdateAbilityPayload payload, CancellationToken cancellationToken = default);
  }
}
