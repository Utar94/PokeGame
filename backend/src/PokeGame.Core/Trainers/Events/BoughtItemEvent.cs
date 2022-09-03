namespace PokeGame.Core.Trainers.Events
{
  public class BoughtItemEvent : AddedItemEvent
  {
    public BoughtItemEvent(Guid itemId, int itemPrice, ushort quantity, Guid userId)
      : base(itemId, quantity, userId)
    {
      ItemPrice = itemPrice;
    }

    public int ItemPrice { get; private set; }
  }
}
