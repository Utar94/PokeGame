using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Regions.Events;

public class RegionDeleted : DomainEvent, INotification
{
  public RegionDeleted()
  {
    IsDeleted = true;
  }
}
