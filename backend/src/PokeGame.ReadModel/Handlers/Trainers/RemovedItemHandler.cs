using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.ReadModel.Handlers.Trainers
{
  internal class RemovedItemHandler : INotificationHandler<RemovedItem>
  {
    private readonly SynchronizeInventory _synchronizeInventory;

    public RemovedItemHandler(SynchronizeInventory synchronizeInventory)
    {
      _synchronizeInventory = synchronizeInventory;
    }

    public async Task Handle(RemovedItem notification, CancellationToken cancellationToken)
    {
      await _synchronizeInventory.ExecuteAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }
}
