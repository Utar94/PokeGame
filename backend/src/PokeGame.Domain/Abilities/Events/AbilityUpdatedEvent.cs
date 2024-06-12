using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace PokeGame.Domain.Abilities.Events;

public class AbilityUpdatedEvent : DomainEvent, INotification
{
  public UniqueNameUnit? UniqueName { get; set; }
  public Modification<DisplayNameUnit>? DisplayName { get; set; }
  public Modification<DescriptionUnit>? Description { get; set; }

  public Modification<UrlUnit>? Reference { get; set; }
  public Modification<NotesUnit>? Notes { get; set; }

  public bool HasChanges => UniqueName != null || DisplayName != null || Description != null
    || Reference != null || Notes != null;
}
