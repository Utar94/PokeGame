using PokeGame.Core.Moves.Payloads;

namespace PokeGame.Core.Moves.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateMovePayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateMovePayload Payload { get; private set; }
  }
}
