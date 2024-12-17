using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Abilities.Events;

public record AbilityUpdated : DomainEvent, INotification
{
  public UniqueName? UniqueName { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<Url>? Link { get; set; }
  public Change<Notes>? Notes { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueName != null || DisplayName != null || Description != null || Link != null || Notes != null;
}
