using Logitar.Portal.Contracts.Search;
using PokeGame.Application.Speciez.Models;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;

namespace PokeGame.Application.Speciez;

public interface ISpeciesQuerier
{
  Task<SpeciesId?> FindIdAsync(UniqueName uniqueName, CancellationToken cancellationToken = default);

  Task<SpeciesModel> ReadAsync(Species species, CancellationToken cancellationToken = default);
  Task<SpeciesModel?> ReadAsync(SpeciesId id, CancellationToken cancellationToken = default);
  Task<SpeciesModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<SpeciesModel?> ReadAsync(string uniqueName, CancellationToken cancellationToken = default);

  Task<SearchResults<SpeciesModel>> SearchAsync(SearchSpeciesPayload payload, CancellationToken cancellationToken = default);
}
