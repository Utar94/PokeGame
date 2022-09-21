using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Trainers;
using PokeGame.Infrastructure.ReadModel.Entities;
using PokeGame.Infrastructure.ReadModel.Handlers.Items;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal class SynchronizeInventory
  {
    private readonly ReadContext _readContext;
    private readonly IRepository _repository;
    private readonly SynchronizeItem _synchronizeItem;
    private readonly SynchronizeTrainer _synchronizeTrainer;

    public SynchronizeInventory(
      ReadContext readContext,
      IRepository repository,
      SynchronizeItem synchronizeItem,
      SynchronizeTrainer synchronizeTrainer
    )
    {
      _readContext = readContext;
      _repository = repository;
      _synchronizeItem = synchronizeItem;
      _synchronizeTrainer = synchronizeTrainer;
    }

    public async Task<InventoryEntity?> ExecuteAsync(Guid trainerId, Guid itemId, int version, CancellationToken cancellationToken = default)
    {
      TrainerEntity? trainerEntity = await _synchronizeTrainer.ExecuteAsync(trainerId, version, cancellationToken);
      if (trainerEntity == null)
      {
        return null;
      }

      ItemEntity? item = await _readContext.Items.SingleOrDefaultAsync(x => x.Id == itemId, cancellationToken)
        ?? await _synchronizeItem.ExecuteAsync(itemId, version: null, cancellationToken);
      if (item == null)
      {
        return null;
      }

      InventoryEntity entity = trainerEntity.Inventory.SingleOrDefault(x => x.ItemId == item.Sid)
        ?? trainerEntity.Add(item);

      Trainer? trainer = await _repository.LoadAsync<Trainer>(trainerId, version, cancellationToken);
      if (trainer == null)
      {
        return null;
      }

      if (trainer.Inventory.TryGetValue(item.Id, out int quantity) && quantity > 0)
      {
        entity.Quantity = quantity;
      }
      else
      {
        trainerEntity.Inventory.Remove(entity);
      }

      await _readContext.SaveChangesAsync(cancellationToken);

      return entity;
    }
  }
}
