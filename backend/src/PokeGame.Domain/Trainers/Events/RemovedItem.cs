using MediatR;

namespace PokeGame.Domain.Trainers.Events
{
  public class RemovedItem : DomainEvent, INotification
  {
    public RemovedItem(Guid itemId, ushort quantity)
    {
      ItemId = itemId;
      Quantity = quantity;
    }

    public Guid ItemId { get; private set; }
    public ushort Quantity { get; private set; }
  }
}
