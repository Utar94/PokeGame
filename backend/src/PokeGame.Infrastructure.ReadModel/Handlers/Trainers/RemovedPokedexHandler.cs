using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal class RemovedPokedexHandler : INotificationHandler<RemovedPokedex>
  {
    private readonly SynchronizePokedex _synchronizePokedex;

    public RemovedPokedexHandler(SynchronizePokedex synchronizePokedex)
    {
      _synchronizePokedex = synchronizePokedex;
    }

    public async Task Handle(RemovedPokedex notification, CancellationToken cancellationToken)
    {
      await _synchronizePokedex.ExecuteAsync(notification.AggregateId, notification.SpeciesId, notification.Version, cancellationToken);
    }
  }
}
