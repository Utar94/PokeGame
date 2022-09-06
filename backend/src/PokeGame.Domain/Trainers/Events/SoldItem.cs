using MediatR;

namespace PokeGame.Domain.Trainers.Events
{
  public class SoldItem : DomainEvent, INotification
  {
    public SoldItem(Guid itemId, int itemPrice, ushort quantity)
    {
      ItemId = itemId;
      ItemPrice = itemPrice;
      Quantity = quantity;
    }

    public Guid ItemId { get; private set; }
    public int ItemPrice { get; private set; }
    public ushort Quantity { get; private set; }
  }
}
