using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.ReadModel.Handlers.Trainers
{
  internal class SavedPokedexHandler : INotificationHandler<SavedPokedex>
  {
    private readonly SynchronizePokedex _synchronizePokedex;

    public SavedPokedexHandler(SynchronizePokedex synchronizePokedex)
    {
      _synchronizePokedex = synchronizePokedex;
    }

    public async Task Handle(SavedPokedex notification, CancellationToken cancellationToken)
    {
      await _synchronizePokedex.ExecuteAsync(notification.AggregateId, notification.SpeciesId, notification.Version, cancellationToken);
    }
  }
}
