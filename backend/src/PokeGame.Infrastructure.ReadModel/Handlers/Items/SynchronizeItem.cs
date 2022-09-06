using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Items;
using PokeGame.Domain.Items.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Items
{
  internal abstract class SynchronizeItem
  {
    protected SynchronizeItem(ReadContext readContext, IRepository<Item> repository)
    {
      ReadContext = readContext;
      Repository = repository;
    }

    protected ReadContext ReadContext { get; }
    protected IRepository<Item> Repository { get; }

    protected async Task SynchronizeAsync(Guid id, int version, CancellationToken cancellationToken)
    {
      Entities.Item? entity = await ReadContext.Items
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity == null)
      {
        entity = new Entities.Item { Id = id };
        ReadContext.Items.Add(entity);
      }
      else if (entity.Version >= version)
      {
        return;
      }

      Item item = await Repository.LoadAsync(id, version, cancellationToken)
        ?? throw new EntityNotFoundException<Item>(id);

      entity.Synchronize(item);

      await ReadContext.SaveChangesAsync(cancellationToken);
    }
  }

  internal class ItemCreatedHandler : SynchronizeItem, INotificationHandler<ItemCreated>
  {
    public ItemCreatedHandler(ReadContext readContext, IRepository<Item> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(ItemCreated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }

  internal class ItemUpdatedHandler : SynchronizeItem, INotificationHandler<ItemUpdated>
  {
    public ItemUpdatedHandler(ReadContext readContext, IRepository<Item> repository)
      : base(readContext, repository)
    {
    }

    public async Task Handle(ItemUpdated notification, CancellationToken cancellationToken)
    {
      await SynchronizeAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
