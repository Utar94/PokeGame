using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Species;
using PokeGame.Domain;
using PokeGame.Domain.Species;

namespace PokeGame.Application.Species;

public interface IPokemonSpeciesQuerier
{
  Task<SpeciesId?> FindIdAsync(UniqueName uniqueName, CancellationToken cancellationToken = default);

  Task<SpeciesModel> ReadAsync(PokemonSpecies species, CancellationToken cancellationToken = default);
  Task<SpeciesModel?> ReadAsync(SpeciesId id, CancellationToken cancellationToken = default);
  Task<SpeciesModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<SpeciesModel?> ReadAsync(int number, CancellationToken cancellationToken = default);
  Task<SpeciesModel?> ReadAsync(string uniqueName, CancellationToken cancellationToken = default);

  Task<SearchResults<SpeciesModel>> SearchAsync(SearchSpeciesPayload payload, CancellationToken cancellationToken = default);
}
