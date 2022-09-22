using MediatR;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Queries
{
  internal class GetSpeciesQueryHandler : IRequestHandler<GetSpeciesQuery, SpeciesModel?>
  {
    private readonly ISpeciesQuerier _querier;

    public GetSpeciesQueryHandler(ISpeciesQuerier querier)
    {
      _querier = querier;
    }

    public async Task<SpeciesModel?> Handle(GetSpeciesQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(request.Id, cancellationToken);
    }
  }
}
