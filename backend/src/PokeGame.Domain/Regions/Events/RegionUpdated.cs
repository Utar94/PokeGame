using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Regions.Events;

public class RegionUpdated : DomainEvent, INotification
{
  public UniqueName? UniqueName { get; }
  public Change<DisplayName>? DisplayName { get; }
  public Change<Description>? Description { get; }

  public Change<Url>? Link { get; }
  public Change<Notes>? Notes { get; }

  public RegionUpdated(
    UniqueName? uniqueName,
    Change<DisplayName>? displayName,
    Change<Description>? description,
    Change<Url>? link,
    Change<Notes>? notes)
  {
    UniqueName = uniqueName;
    DisplayName = displayName;
    Description = description;
    Link = link;
    Notes = notes;
  }
}
