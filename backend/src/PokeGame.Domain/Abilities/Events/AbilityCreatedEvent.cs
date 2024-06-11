using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace PokeGame.Domain.Abilities.Events;

public class AbilityCreatedEvent : DomainEvent, INotification
{
  public UniqueNameUnit UniqueName { get; }

  public AbilityCreatedEvent(UniqueNameUnit uniqueName)
  {
    UniqueName = uniqueName;
  }
}
