using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Trainers;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal abstract class SynchronizeInventory
  {
    protected SynchronizeInventory(ReadContext readContext, IRepository<Trainer> repository)
    {
      ReadContext = readContext;
      Repository = repository;
    }

    protected ReadContext ReadContext { get; }
    protected IRepository<Trainer> Repository { get; }

    protected async Task SynchronizeAsync(Guid trainerId, Guid itemId, int version, CancellationToken cancellationToken)
    {
      Trainer trainer = await Repository.LoadAsync(trainerId, version, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainerId);

      Entities.Trainer trainerEntity = await ReadContext.Trainers
        .Include(x => x.Inventory)
        .SingleAsync(x => x.Id == trainer.Id, cancellationToken);
      if (trainerEntity.Version < version)
      {
        trainerEntity.Synchronize(trainer);
      }

      Entities.Item item = await ReadContext.Items
        .SingleAsync(x => x.Id == itemId, cancellationToken);

      Entities.Inventory? entity = trainerEntity.Inventory.SingleOrDefault(x => x.ItemId == item.Sid);

      if (trainer.Inventory.TryGetValue(item.Id, out int quantity))
      {
        if (entity == null)
        {
          entity = new Entities.Inventory
          {
            TrainerId = trainerEntity.Sid,
            ItemId = item.Sid
          };
          trainerEntity.Inventory.Add(entity);
        }

        entity.Quantity = quantity;
      }
      else if (entity != null)
      {
        trainerEntity.Inventory.Remove(entity);
      }

      await ReadContext.SaveChangesAsync(cancellationToken);
    }
  }

  internal class AddedItemHandler : SynchronizeInventory, INotificationHandler<AddedItem>
  {
    public AddedItemHandler(ReadContext readContext, IRepository<Trainer> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(AddedItem notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }

  internal class BoughtItemHandler : SynchronizeInventory, INotificationHandler<BoughtItem>
  {
    public BoughtItemHandler(ReadContext readContext, IRepository<Trainer> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(BoughtItem notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }

  internal class RemovedItemHandler : SynchronizeInventory, INotificationHandler<RemovedItem>
  {
    public RemovedItemHandler(ReadContext readContext, IRepository<Trainer> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(RemovedItem notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }

  internal class SoldItemHandler : SynchronizeInventory, INotificationHandler<SoldItem>
  {
    public SoldItemHandler(ReadContext readContext, IRepository<Trainer> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(SoldItem notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }
}
