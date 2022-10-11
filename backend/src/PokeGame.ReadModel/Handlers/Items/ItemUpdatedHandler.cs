using MediatR;
using PokeGame.Domain.Items.Events;

namespace PokeGame.ReadModel.Handlers.Items
{
  internal class ItemUpdatedHandler : INotificationHandler<ItemUpdated>
  {
    private readonly SynchronizeItem _synchronizeItem;

    public ItemUpdatedHandler(SynchronizeItem synchronizeItem)
    {
      _synchronizeItem = synchronizeItem;
    }

    public async Task Handle(ItemUpdated notification, CancellationToken cancellationToken)
    {
      await _synchronizeItem.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
