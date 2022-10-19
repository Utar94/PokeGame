using MediatR;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Queries
{
  public class GetSpeciesQuery : IRequest<SpeciesModel?>
  {
    public GetSpeciesQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
