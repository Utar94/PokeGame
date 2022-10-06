using MediatR;
using PokeGame.Domain.Items.Payloads;

namespace PokeGame.Domain.Items.Events
{
  public class ItemUpdated : DomainEvent, INotification
  {
    public ItemUpdated(UpdateItemPayload payload)
    {
      Payload = payload;
    }

    public UpdateItemPayload Payload { get; private set; }
  }
}
