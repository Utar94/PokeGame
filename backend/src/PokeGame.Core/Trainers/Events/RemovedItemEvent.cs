namespace PokeGame.Core.Trainers.Events
{
  public class RemovedItemEvent : UpdatedEventBase
  {
    public RemovedItemEvent(Guid itemId, ushort quantity, Guid userId) : base(userId)
    {
      ItemId = itemId;
      Quantity = quantity;
    }

    public Guid ItemId { get; private set; }
    public ushort Quantity { get; private set; }
  }
}
