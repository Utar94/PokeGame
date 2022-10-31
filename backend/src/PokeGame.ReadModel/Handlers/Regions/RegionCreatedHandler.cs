using MediatR;
using PokeGame.Domain.Regions.Events;

namespace PokeGame.ReadModel.Handlers.Regions
{
  internal class RegionCreatedHandler : INotificationHandler<RegionCreated>
  {
    private readonly SynchronizeRegion _synchronizeRegion;

    public RegionCreatedHandler(SynchronizeRegion synchronizeRegion)
    {
      _synchronizeRegion = synchronizeRegion;
    }

    public async Task Handle(RegionCreated notification, CancellationToken cancellationToken)
    {
      await _synchronizeRegion.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
