using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Application.Speciez.Models;

namespace PokeGame.Application.Speciez.Queries;

public record SearchSpeciesQuery(SearchSpeciesPayload Payload) : IRequest<SearchResults<SpeciesModel>>;

internal class SearchSpeciesQueryHandler : IRequestHandler<SearchSpeciesQuery, SearchResults<SpeciesModel>>
{
  private readonly ISpeciesQuerier _speciesQuerier;

  public SearchSpeciesQueryHandler(ISpeciesQuerier speciesQuerier)
  {
    _speciesQuerier = speciesQuerier;
  }

  public async Task<SearchResults<SpeciesModel>> Handle(SearchSpeciesQuery query, CancellationToken cancellationToken)
  {
    return await _speciesQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
