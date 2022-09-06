using PokeGame.Application.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species
{
  public interface ISpeciesService
  {
    Task<SpeciesModel> CreateAsync(CreateSpeciesPayload payload, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<SpeciesModel>> GetAsync(string? search = null, PokemonType? type = null,
      SpeciesSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<SpeciesModel> UpdateAsync(Guid id, UpdateSpeciesPayload payload, CancellationToken cancellationToken = default);
  }
}
