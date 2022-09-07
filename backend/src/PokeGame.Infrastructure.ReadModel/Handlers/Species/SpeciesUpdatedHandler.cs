using MediatR;
using PokeGame.Domain.Species.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Species
{
  internal class SpeciesUpdatedHandler : INotificationHandler<SpeciesUpdated>
  {
    private readonly SynchronizeSpecies _synchronizeSpecies;

    public SpeciesUpdatedHandler(SynchronizeSpecies synchronizeSpecies)
    {
      _synchronizeSpecies = synchronizeSpecies;
    }

    public async Task Handle(SpeciesUpdated notification, CancellationToken cancellationToken)
    {
      await _synchronizeSpecies.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
