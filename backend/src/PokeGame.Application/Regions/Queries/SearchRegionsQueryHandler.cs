using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

internal class SearchRegionsQueryHandler : IRequestHandler<SearchRegionsQuery, SearchResults<Region>>
{
  private readonly IRegionQuerier _regionQuerier;

  public SearchRegionsQueryHandler(IRegionQuerier regionQuerier)
  {
    _regionQuerier = regionQuerier;
  }

  public async Task<SearchResults<Region>> Handle(SearchRegionsQuery query, CancellationToken cancellationToken)
  {
    return await _regionQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
