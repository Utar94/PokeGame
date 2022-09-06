using MediatR;

namespace PokeGame.Domain.Trainers.Events
{
  public class AddedItem : DomainEvent, INotification
  {
    public AddedItem(Guid itemId, ushort quantity)
    {
      ItemId = itemId;
      Quantity = quantity;
    }

    public Guid ItemId { get; private set; }
    public ushort Quantity { get; private set; }
  }
}
