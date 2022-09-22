using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Queries
{
  internal class GetSpeciesListQueryHandler : IRequestHandler<GetSpeciesListQuery, ListModel<SpeciesModel>>
  {
    private readonly ISpeciesQuerier _querier;

    public GetSpeciesListQueryHandler(ISpeciesQuerier querier)
    {
      _querier = querier;
    }

    public async Task<ListModel<SpeciesModel>> Handle(GetSpeciesListQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(request.Search, request.Type,
        request.Sort, request.Desc,
        request.Index, request.Count,
        cancellationToken);
    }
  }
}
