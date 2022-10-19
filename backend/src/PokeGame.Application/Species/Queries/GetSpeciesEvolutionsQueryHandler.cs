using MediatR;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Queries
{
  internal class GetSpeciesEvolutionsQueryHandler : IRequestHandler<GetSpeciesEvolutionsQuery, IEnumerable<EvolutionModel>>
  {
    private readonly ISpeciesQuerier _querier;

    public GetSpeciesEvolutionsQueryHandler(ISpeciesQuerier querier)
    {
      _querier = querier;
    }

    public async Task<IEnumerable<EvolutionModel>> Handle(GetSpeciesEvolutionsQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetEvolutionsAsync(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(request.Id);
    }
  }
}
