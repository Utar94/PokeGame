using MediatR;
using PokeGame.Domain.Regions.Payloads;

namespace PokeGame.Domain.Regions.Events
{
  public class RegionCreated : DomainEvent, INotification
  {
    public RegionCreated(CreateRegionPayload payload)
    {
      Payload = payload;
    }

    public CreateRegionPayload Payload { get; private set; }
  }
}
