using MediatR;
using PokeGame.Application.Pokedex.Models;

namespace PokeGame.Application.Pokedex.Mutations
{
  public class SavePokedexEntryMutation : IRequest<PokedexModel>
  {
    public SavePokedexEntryMutation(Guid trainerId, Guid speciesId, bool hasCaught)
    {
      HasCaught = hasCaught;
      SpeciesId = speciesId;
      TrainerId = trainerId;
    }

    public bool HasCaught { get; }
    public Guid SpeciesId { get; }
    public Guid TrainerId { get; }
  }
}
