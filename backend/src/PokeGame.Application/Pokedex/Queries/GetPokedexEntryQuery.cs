using MediatR;
using PokeGame.Application.Pokedex.Models;

namespace PokeGame.Application.Pokedex.Queries
{
  public class GetPokedexEntryQuery : IRequest<PokedexModel?>
  {
    public GetPokedexEntryQuery(Guid trainerId, Guid speciesId)
    {
      SpeciesId = speciesId;
      TrainerId = trainerId;
    }

    public Guid SpeciesId { get; }
    public Guid TrainerId { get; }
  }
}
