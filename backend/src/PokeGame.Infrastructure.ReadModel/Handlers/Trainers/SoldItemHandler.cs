using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal class SoldItemHandler : INotificationHandler<SoldItem>
  {
    private readonly SynchronizeInventory _synchronizeInventory;

    public SoldItemHandler(SynchronizeInventory synchronizeInventory)
    {
      _synchronizeInventory = synchronizeInventory;
    }

    public async Task Handle(SoldItem notification, CancellationToken cancellationToken)
    {
      await _synchronizeInventory.ExecuteAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }
}
