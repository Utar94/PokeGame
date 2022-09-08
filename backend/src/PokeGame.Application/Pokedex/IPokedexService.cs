using PokeGame.Application.Models;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Pokedex
{
  public interface IPokedexService
  {
    Task DeleteAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken = default);
    Task<ListModel<PokedexModel>> GetAsync(Guid trainerId, bool? hasCaught = null, string? search = null, PokemonType? type = null,
      PokedexSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<PokedexModel?> GetAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken = default);
    Task<PokedexModel> SaveAsync(Guid trainerId, Guid speciesId, bool hasCaught = false, CancellationToken cancellationToken = default);
  }
}
