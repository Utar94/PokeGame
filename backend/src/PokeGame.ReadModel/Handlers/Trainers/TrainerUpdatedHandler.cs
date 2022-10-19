using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.ReadModel.Handlers.Trainers
{
  internal class TrainerUpdatedHandler : INotificationHandler<TrainerUpdated>
  {
    private readonly SynchronizeTrainer _synchronizeTrainer;

    public TrainerUpdatedHandler(SynchronizeTrainer synchronizeTrainer)
    {
      _synchronizeTrainer = synchronizeTrainer;
    }

    public async Task Handle(TrainerUpdated notification, CancellationToken cancellationToken)
    {
      await _synchronizeTrainer.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
