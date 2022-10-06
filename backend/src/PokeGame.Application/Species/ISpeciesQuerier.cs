using PokeGame.Application.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Species
{
  public interface ISpeciesQuerier
  {
    Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SpeciesModel?> GetAsync(int number, CancellationToken cancellationToken = default);
    Task<SpeciesModel?> GetAsync(Region region, int number, CancellationToken cancellationToken = default);
    Task<EvolutionModel?> GetEvolutionAsync(Guid id, Guid speciesId, CancellationToken cancellationToken = default);
    Task<IEnumerable<EvolutionModel>?> GetEvolutionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<SpeciesModel>> GetPagedAsync(Region? region = null, string? search = null, PokemonType? type = null,
      SpeciesSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
