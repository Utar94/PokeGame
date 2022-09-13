using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon
{
  public interface IPokemonQuerier
  {
    Task<PokemonModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<PokemonModel>> GetPagedAsync(PokemonGender? gender = null, byte? inBox = null, bool? inParty = null, bool? isWild = null, string? search = null, Guid? speciesId = null, Guid? trainerId = null,
      PokemonSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
