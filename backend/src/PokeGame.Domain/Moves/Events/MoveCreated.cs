using MediatR;
using PokeGame.Domain.Moves.Payloads;

namespace PokeGame.Domain.Moves.Events
{
  public class MoveCreated : DomainEvent, INotification
  {
    public MoveCreated(CreateMovePayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateMovePayload Payload { get; private set; }
  }
}
