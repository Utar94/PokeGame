using MediatR;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Species.Events
{
  public class SpeciesUpdated : DomainEvent, INotification
  {
    public SpeciesUpdated(UpdateSpeciesPayload payload)
    {
      Payload = payload;
    }

    public UpdateSpeciesPayload Payload { get; private set; }
  }
}
