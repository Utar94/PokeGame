using MediatR;
using PokeGame.Domain.Items.Payloads;

namespace PokeGame.Domain.Items.Events
{
  public class ItemCreated : DomainEvent, INotification
  {
    public ItemCreated(CreateItemPayload payload)
    {
      Payload = payload;
    }

    public CreateItemPayload Payload { get; private set; }
  }
}
