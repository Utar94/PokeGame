using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace PokeGame.Domain.Items.Events;

public class ItemUpdatedEvent : DomainEvent, INotification
{
  public Modification<int?>? Price { get; set; }

  public UniqueNameUnit? UniqueName { get; set; }
  public Modification<DisplayNameUnit>? DisplayName { get; set; }
  public Modification<DescriptionUnit>? Description { get; set; }
  public Modification<UrlUnit>? Picture { get; set; }

  public Modification<UrlUnit>? Reference { get; set; }
  public Modification<NotesUnit>? Notes { get; set; }

  public bool HasChanges => Price != null
    || UniqueName != null || DisplayName != null || Description != null || Picture != null
    || Reference != null || Notes != null;
}
