using PokeGame.Core.Items.Payloads;

namespace PokeGame.Core.Items.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateItemPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateItemPayload Payload { get; private set; }
  }
}
