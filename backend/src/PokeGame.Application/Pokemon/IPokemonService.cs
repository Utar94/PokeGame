using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon
{
  public interface IPokemonService
  {
    Task<PokemonModel> CatchAsync(Guid id, CatchPokemonPayload payload, CancellationToken cancellationToken = default);
    Task<PokemonModel> CreateAsync(CreatePokemonPayload payload, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PokemonModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<PokemonModel>> GetAsync(PokemonGender? gender = null, string? search = null, Guid? speciesId = null, Guid? trainerId = null,
      PokemonSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<PokemonModel> HealAsync(Guid id, HealPokemonPayload payload, CancellationToken cancellationToken = default);
    Task<PokemonModel> UpdateAsync(Guid id, UpdatePokemonPayload payload, CancellationToken cancellationToken = default);
  }
}
