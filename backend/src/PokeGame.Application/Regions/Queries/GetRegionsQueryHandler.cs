using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions.Queries
{
  internal class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, ListModel<RegionModel>>
  {
    private readonly IRegionQuerier _querier;

    public GetRegionsQueryHandler(IRegionQuerier querier)
    {
      _querier = querier;
    }

    public async Task<ListModel<RegionModel>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(request.Search,
        request.Sort, request.Desc,
        request.Index, request.Count,
        cancellationToken);
    }
  }
}
