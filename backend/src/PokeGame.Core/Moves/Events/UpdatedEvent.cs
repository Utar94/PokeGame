using PokeGame.Core.Moves.Payloads;

namespace PokeGame.Core.Moves.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateMovePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateMovePayload Payload { get; private set; }
  }
}
