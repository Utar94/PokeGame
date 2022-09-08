using MediatR;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Moves
{
  internal class MoveCreatedHandler : INotificationHandler<MoveCreated>
  {
    private readonly SynchronizeMove _synchronizeMove;

    public MoveCreatedHandler(SynchronizeMove synchronizeMove)
    {
      _synchronizeMove = synchronizeMove;
    }

    public async Task Handle(MoveCreated notification, CancellationToken cancellationToken)
    {
      await _synchronizeMove.ExecuteAsync(notification.AggregateId, notification.Version, cancellationToken);
    }
  }
}
