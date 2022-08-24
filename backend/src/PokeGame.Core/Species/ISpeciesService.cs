using PokeGame.Core.Models;
using PokeGame.Core.Species.Models;
using PokeGame.Core.Species.Payloads;

namespace PokeGame.Core.Species
{
  public interface ISpeciesService
  {
    Task<SpeciesModel> CreateAsync(CreateSpeciesPayload payload, CancellationToken cancellationToken = default);
    Task<SpeciesModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<SpeciesModel>> GetAsync(string? search = null, PokemonType? type = null,
      SpeciesSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<SpeciesModel> UpdateAsync(Guid id, UpdateSpeciesPayload payload, CancellationToken cancellationToken = default);
  }
}
