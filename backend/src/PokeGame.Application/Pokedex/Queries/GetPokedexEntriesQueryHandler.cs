using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Pokedex.Models;

namespace PokeGame.Application.Pokedex.Queries
{
  internal class GetPokedexEntriesQueryHandler : IRequestHandler<GetPokedexEntriesQuery, ListModel<PokedexModel>>
  {
    private readonly IPokedexQuerier _querier;

    public GetPokedexEntriesQueryHandler(IPokedexQuerier querier)
    {
      _querier = querier;
    }

    public async Task<ListModel<PokedexModel>> Handle(GetPokedexEntriesQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(request.TrainerId, request.HasCaught, request.RegionId, request.Search, request.Type,
        request.Sort, request.Desc,
        request.Index, request.Count,
        cancellationToken);
    }
  }
}
