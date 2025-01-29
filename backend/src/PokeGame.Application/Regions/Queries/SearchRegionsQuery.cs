using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions.Queries;

public record SearchRegionsQuery(SearchRegionsPayload Payload) : IRequest<SearchResults<RegionModel>>;

internal class SearchRegionsQueryHandler : IRequestHandler<SearchRegionsQuery, SearchResults<RegionModel>>
{
  private readonly IRegionQuerier _regionQuerier;

  public SearchRegionsQueryHandler(IRegionQuerier regionQuerier)
  {
    _regionQuerier = regionQuerier;
  }

  public async Task<SearchResults<RegionModel>> Handle(SearchRegionsQuery query, CancellationToken cancellationToken)
  {
    return await _regionQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
