using MediatR;
using PokeGame.Domain.Moves.Payloads;

namespace PokeGame.Domain.Moves.Events
{
  public class MoveUpdated : DomainEvent, INotification
  {
    public MoveUpdated(UpdateMovePayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateMovePayload Payload { get; private set; }
  }
}
