using MediatR;
using PokeGame.Domain.Regions.Payloads;

namespace PokeGame.Domain.Regions.Events
{
  public class RegionUpdated : DomainEvent, INotification
  {
    public RegionUpdated(UpdateRegionPayload payload)
    {
      Payload = payload;
    }

    public UpdateRegionPayload Payload { get; private set; }
  }
}
