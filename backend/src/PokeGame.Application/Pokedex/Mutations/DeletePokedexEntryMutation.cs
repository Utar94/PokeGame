using MediatR;

namespace PokeGame.Application.Pokedex.Mutations
{
  public class DeletePokedexEntryMutation : IRequest
  {
    public DeletePokedexEntryMutation(Guid trainerId, Guid speciesId)
    {
      SpeciesId = speciesId;
      TrainerId = trainerId;
    }

    public Guid SpeciesId { get; }
    public Guid TrainerId { get; }
  }
}
