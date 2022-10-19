using MediatR;
using PokeGame.Application.Inventories.Models;

namespace PokeGame.Application.Inventories.Mutations
{
  public class AddInventoryMutation : IRequest<InventoryModel>
  {
    public AddInventoryMutation(Guid trainerId, Guid itemId, ushort quantity, bool buy = false)
    {
      Buy = buy;
      ItemId = itemId;
      Quantity = quantity;
      TrainerId = trainerId;
    }

    public bool Buy { get; }
    public Guid ItemId { get; }
    public ushort Quantity { get; }
    public Guid TrainerId { get; }
  }
}
