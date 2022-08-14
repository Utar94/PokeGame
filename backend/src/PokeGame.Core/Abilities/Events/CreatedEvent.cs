using PokeGame.Core.Abilities.Payloads;

namespace PokeGame.Core.Abilities.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateAbilityPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateAbilityPayload Payload { get; private set; }
  }
}
