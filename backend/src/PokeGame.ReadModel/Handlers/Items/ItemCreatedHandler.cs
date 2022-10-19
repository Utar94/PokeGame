using MediatR;
using PokeGame.Domain.Items.Events;

namespace PokeGame.ReadModel.Handlers.Items
{
  internal class ItemCreatedHandler : INotificationHandler<ItemCreated>
  {
    private readonly SynchronizeItem _synchronizeItem;

    public ItemCreatedHandler(SynchronizeItem synchronizeItem)
    {
      _synchronizeItem = synchronizeItem;
    }

    public async Task Handle(ItemCreated notification, CancellationToken cancellationToken)
    {
      await _synchronizeItem.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
