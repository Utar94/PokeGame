using MediatR;
using PokeGame.Domain.Regions.Events;

namespace PokeGame.ReadModel.Handlers.Regions
{
  internal class RegionUpdatedHandler : INotificationHandler<RegionUpdated>
  {
    private readonly SynchronizeRegion _synchronizeRegion;

    public RegionUpdatedHandler(SynchronizeRegion synchronizeRegion)
    {
      _synchronizeRegion = synchronizeRegion;
    }

    public async Task Handle(RegionUpdated notification, CancellationToken cancellationToken)
    {
      await _synchronizeRegion.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
