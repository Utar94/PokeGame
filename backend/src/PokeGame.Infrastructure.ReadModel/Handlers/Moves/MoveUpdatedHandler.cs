using MediatR;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Moves
{
  internal class MoveUpdatedHandler : INotificationHandler<MoveUpdated>
  {
    private readonly SynchronizeMove _synchronizeMove;

    public MoveUpdatedHandler(SynchronizeMove synchronizeMove)
    {
      _synchronizeMove = synchronizeMove;
    }

    public async Task Handle(MoveUpdated notification, CancellationToken cancellationToken)
    {
      await _synchronizeMove.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
