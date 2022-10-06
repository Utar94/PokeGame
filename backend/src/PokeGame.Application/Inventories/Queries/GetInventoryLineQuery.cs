using MediatR;
using PokeGame.Application.Inventories.Models;

namespace PokeGame.Application.Inventories.Queries
{
  public class GetInventoryLineQuery : IRequest<InventoryModel>
  {
    public GetInventoryLineQuery(Guid trainerId, Guid itemId)
    {
      ItemId = itemId;
      TrainerId = trainerId;
    }

    public Guid ItemId { get; }
    public Guid TrainerId { get; }
  }
}
