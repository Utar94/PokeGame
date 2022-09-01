using PokeGame.Core.Items.Payloads;

namespace PokeGame.Core.Items.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateItemPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateItemPayload Payload { get; private set; }
  }
}
