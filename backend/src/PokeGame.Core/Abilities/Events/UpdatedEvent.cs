using PokeGame.Core.Abilities.Payloads;

namespace PokeGame.Core.Abilities.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateAbilityPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateAbilityPayload Payload { get; private set; }
  }
}
