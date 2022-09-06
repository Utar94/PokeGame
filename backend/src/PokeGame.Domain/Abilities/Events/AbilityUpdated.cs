using MediatR;
using PokeGame.Domain.Abilities.Payloads;

namespace PokeGame.Domain.Abilities.Events
{
  public class AbilityUpdated : DomainEvent, INotification
  {
    public AbilityUpdated(UpdateAbilityPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateAbilityPayload Payload { get; private set; }
  }
}
