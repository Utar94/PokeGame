using MediatR;
using PokeGame.Application.Inventories.Models;

namespace PokeGame.Application.Inventories.Mutations
{
  public class RemoveInventoryMutation : IRequest<InventoryModel>
  {
    public RemoveInventoryMutation(Guid trainerId, Guid itemId, ushort quantity, bool sell = false)
    {
      ItemId = itemId;
      Quantity = quantity;
      Sell = sell;
      TrainerId = trainerId;
    }

    public Guid ItemId { get; }
    public ushort Quantity { get; }
    public bool Sell { get; }
    public Guid TrainerId { get; }
  }
}
