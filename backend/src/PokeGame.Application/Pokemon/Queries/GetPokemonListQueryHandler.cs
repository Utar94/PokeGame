using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;

namespace PokeGame.Application.Pokemon.Queries
{
  internal class GetPokemonListQueryHandler : IRequestHandler<GetPokemonListQuery, ListModel<PokemonModel>>
  {
    private readonly IPokemonQuerier _querier;

    public GetPokemonListQueryHandler(IPokemonQuerier querier)
    {
      _querier = querier;
    }

    public async Task<ListModel<PokemonModel>> Handle(GetPokemonListQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(request.Gender, request.InBox, request.InParty, request.IsWild, request.Search, request.SpeciesId, request.TrainerId,
        request.Sort, request.Desc,
        request.Index, request.Count,
        cancellationToken);
    }
  }
}
