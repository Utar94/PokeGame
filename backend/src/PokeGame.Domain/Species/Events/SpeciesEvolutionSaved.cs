using MediatR;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Species.Events
{
  public class SpeciesEvolutionSaved : DomainEvent, INotification
  {
    public SpeciesEvolutionSaved(Guid speciesId, SaveEvolutionPayload payload)
    {
      SpeciesId = speciesId;
      Payload = payload;
    }

    public Guid SpeciesId { get; }
    public SaveEvolutionPayload Payload { get; }
  }
}
