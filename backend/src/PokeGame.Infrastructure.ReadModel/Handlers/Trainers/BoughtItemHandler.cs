using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal class BoughtItemHandler : INotificationHandler<BoughtItem>
  {
    private readonly SynchronizeInventory _synchronizeInventory;

    public BoughtItemHandler(SynchronizeInventory synchronizeInventory)
    {
      _synchronizeInventory = synchronizeInventory;
    }

    public async Task Handle(BoughtItem notification, CancellationToken cancellationToken)
    {
      await _synchronizeInventory.ExecuteAsync(notification.AggregateId, notification.ItemId, notification.Version, cancellationToken);
    }
  }
}
