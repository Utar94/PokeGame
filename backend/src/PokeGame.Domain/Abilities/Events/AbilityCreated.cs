using MediatR;
using PokeGame.Domain.Abilities.Payloads;

namespace PokeGame.Domain.Abilities.Events
{
  public class AbilityCreated : DomainEvent, INotification
  {
    public AbilityCreated(CreateAbilityPayload payload)
    {
      Payload = payload;
    }

    public CreateAbilityPayload Payload { get; private set; }
  }
}
