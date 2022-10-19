using MediatR;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Domain.Species.Events
{
  public class SpeciesCreated : DomainEvent, INotification
  {
    public SpeciesCreated(CreateSpeciesPayload payload)
    {
      Payload = payload;
    }

    public CreateSpeciesPayload Payload { get; private set; }
  }
}
