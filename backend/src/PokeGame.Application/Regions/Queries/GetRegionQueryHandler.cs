using MediatR;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Application.Regions.Queries
{
  internal class GetRegionQueryHandler : IRequestHandler<GetRegionQuery, RegionModel?>
  {
    private readonly IRegionQuerier _querier;

    public GetRegionQueryHandler(IRegionQuerier querier)
    {
      _querier = querier;
    }

    public async Task<RegionModel?> Handle(GetRegionQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(request.Id, cancellationToken);
    }
  }
}
