using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Regions.Events;

public class RegionCreated : DomainEvent, INotification
{
  public UniqueName UniqueName { get; }

  public RegionCreated(UniqueName uniqueName)
  {
    UniqueName = uniqueName;
  }
}
