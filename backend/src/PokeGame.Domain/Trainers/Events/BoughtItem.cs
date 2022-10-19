using MediatR;

namespace PokeGame.Domain.Trainers.Events
{
  public class BoughtItem : DomainEvent, INotification
  {
    public BoughtItem(Guid itemId, int itemPrice, ushort quantity)
    {
      ItemId = itemId;
      ItemPrice = itemPrice;
      Quantity = quantity;
    }

    public Guid ItemId { get; private set; }
    public int ItemPrice { get; private set; }
    public int Money { get; private set; }
    public ushort Quantity { get; private set; }
  }
}
