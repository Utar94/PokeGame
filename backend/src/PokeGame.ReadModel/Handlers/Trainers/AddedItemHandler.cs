using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.ReadModel.Handlers.Trainers
{
  internal class AddedItemHandler : INotificationHandler<AddedItem>
  {
    private readonly SynchronizeInventory _synchronizeInventory;

    public AddedItemHandler(SynchronizeInventory synchronizeInventory)
    {
      _synchronizeInventory = synchronizeInventory;
    }

    public async Task Handle(AddedItem notification, CancellationToken cancellationToken)
    {
      await _synchronizeInventory.ExecuteAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }
}
