using MediatR;
using PokeGame.Application.Pokedex.Models;

namespace PokeGame.Application.Pokedex.Queries
{
  internal class GetPokedexEntryQueryHandler : IRequestHandler<GetPokedexEntryQuery, PokedexModel?>
  {
    private readonly IPokedexQuerier _querier;

    public GetPokedexEntryQueryHandler(IPokedexQuerier querier)
    {
      _querier = querier;
    }

    public async Task<PokedexModel?> Handle(GetPokedexEntryQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(request.TrainerId, request.SpeciesId, cancellationToken);
    }
  }
}
