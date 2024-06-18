using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace PokeGame.Domain.Regions.Events;

public class RegionCreatedEvent : DomainEvent, INotification
{
  public UniqueNameUnit UniqueName { get; }

  public RegionCreatedEvent(UniqueNameUnit uniqueName)
  {
    UniqueName = uniqueName;
  }
}
