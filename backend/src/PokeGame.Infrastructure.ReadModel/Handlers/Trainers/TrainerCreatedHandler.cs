using MediatR;
using PokeGame.Domain.Trainers.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal class TrainerCreatedHandler : INotificationHandler<TrainerCreated>
  {
    private readonly SynchronizeTrainer _synchronizeTrainer;

    public TrainerCreatedHandler(SynchronizeTrainer synchronizeTrainer)
    {
      _synchronizeTrainer = synchronizeTrainer;
    }

    public async Task Handle(TrainerCreated notification, CancellationToken cancellationToken)
    {
      await _synchronizeTrainer.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
