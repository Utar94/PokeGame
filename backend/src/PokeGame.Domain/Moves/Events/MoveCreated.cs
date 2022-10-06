using MediatR;
using PokeGame.Domain.Moves.Payloads;

namespace PokeGame.Domain.Moves.Events
{
  public class MoveCreated : DomainEvent, INotification
  {
    public MoveCreated(CreateMovePayload payload)
    {
      Payload = payload;
    }

    public CreateMovePayload Payload { get; private set; }
  }
}
