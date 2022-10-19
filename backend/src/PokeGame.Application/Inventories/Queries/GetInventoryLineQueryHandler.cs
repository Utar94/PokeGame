using MediatR;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Items;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Inventories.Queries
{
  internal class GetInventoryLineQueryHandler : IRequestHandler<GetInventoryLineQuery, InventoryModel>
  {
    private readonly IItemQuerier _itemQuerier;
    private readonly IInventoryQuerier _querier;
    private readonly ITrainerQuerier _trainerQuerier;

    public GetInventoryLineQueryHandler(
      IItemQuerier itemQuerier,
      IInventoryQuerier querier,
      ITrainerQuerier trainerQuerier
    )
    {
      _itemQuerier = itemQuerier;
      _querier = querier;
      _trainerQuerier = trainerQuerier;
    }

    public async Task<InventoryModel> Handle(GetInventoryLineQuery request, CancellationToken cancellationToken)
    {
      TrainerModel trainer = await _trainerQuerier.GetAsync(request.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.TrainerId);
      ItemModel item = await _itemQuerier.GetAsync(request.ItemId, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(request.ItemId);

      InventoryModel? model = await _querier.GetAsync(trainer.Id, item.Id, cancellationToken);
      if (model == null)
      {
        return new InventoryModel
        {
          Item = item,
          Quantity = 0
        };
      }

      return model;
    }
  }
}
