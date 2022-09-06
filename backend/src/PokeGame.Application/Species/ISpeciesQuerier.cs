using PokeGame.Application.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Species
{
  public interface ISpeciesQuerier
  {
    Task<SpeciesModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<SpeciesModel>> GetPagedAsync(string? search = null, PokemonType? type = null,
      SpeciesSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
