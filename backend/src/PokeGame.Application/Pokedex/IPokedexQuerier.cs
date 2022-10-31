using PokeGame.Application.Models;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Pokedex
{
  public interface IPokedexQuerier
  {
    Task<PokedexModel?> GetAsync(Guid trainerId, Guid speciesId, CancellationToken cancellationToken = default);
    Task<ListModel<PokedexModel>> GetPagedAsync(Guid trainerId, bool? hasCaught = null, Guid? regionId = null, string? search = null, PokemonType? type = null,
      PokedexSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
