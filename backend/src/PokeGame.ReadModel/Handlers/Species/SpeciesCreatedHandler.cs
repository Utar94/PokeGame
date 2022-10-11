using MediatR;
using PokeGame.Domain.Species.Events;

namespace PokeGame.ReadModel.Handlers.Species
{
  internal class SpeciesCreatedHandler : INotificationHandler<SpeciesCreated>
  {
    private readonly SynchronizeSpecies _synchronizeSpecies;

    public SpeciesCreatedHandler(SynchronizeSpecies synchronizeSpecies)
    {
      _synchronizeSpecies = synchronizeSpecies;
    }

    public async Task Handle(SpeciesCreated notification, CancellationToken cancellationToken)
    {
      await _synchronizeSpecies.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
