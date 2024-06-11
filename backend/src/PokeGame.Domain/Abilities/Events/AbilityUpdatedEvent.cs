using Logitar.EventSourcing;
using MediatR;
using PokeGame.Contracts;

namespace PokeGame.Domain.Abilities.Events;

public class AbilityUpdatedEvent : DomainEvent, INotification
{
  public UniqueNameUnit? UniqueName { get; set; }
  public Change<DisplayNameUnit>? DisplayName { get; set; }
  public Change<DescriptionUnit>? Description { get; set; }

  public Change<ReferenceUnit>? Reference { get; set; }
  public Change<NotesUnit>? Notes { get; set; }

  public bool HasChanges => UniqueName != null || DisplayName != null || Description != null
    || Reference != null || Notes != null;
}
