using MediatR;
using PokeGame.Application.Species.Models;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species.Mutations
{
  public class SaveSpeciesEvolutionMutation : IRequest<EvolutionModel>
  {
    public SaveSpeciesEvolutionMutation(Guid id, Guid speciesId, SaveEvolutionPayload payload)
    {
      Id = id;
      SpeciesId = speciesId;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Guid Id { get; }
    public Guid SpeciesId { get; }
    public SaveEvolutionPayload Payload { get; }
  }
}
