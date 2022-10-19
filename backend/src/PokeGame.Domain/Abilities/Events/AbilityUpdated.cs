using MediatR;
using PokeGame.Domain.Abilities.Payloads;

namespace PokeGame.Domain.Abilities.Events
{
  public class AbilityUpdated : DomainEvent, INotification
  {
    public AbilityUpdated(UpdateAbilityPayload payload)
    {
      Payload = payload;
    }

    public UpdateAbilityPayload Payload { get; private set; }
  }
}
