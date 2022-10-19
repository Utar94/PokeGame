using MediatR;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Queries
{
  public class GetSpeciesEvolutionsQuery : IRequest<IEnumerable<EvolutionModel>>
  {
    public GetSpeciesEvolutionsQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
