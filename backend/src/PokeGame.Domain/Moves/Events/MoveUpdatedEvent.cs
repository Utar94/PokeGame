using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace PokeGame.Domain.Moves.Events;

public class MoveUpdatedEvent : DomainEvent, INotification
{
  public UniqueNameUnit? UniqueName { get; set; }
  public Modification<DisplayNameUnit>? DisplayName { get; set; }
  public Modification<DescriptionUnit>? Description { get; set; }

  public Modification<int?>? Accuracy { get; set; }
  public Modification<int?>? Power { get; set; }
  public int? PowerPoints { get; set; }

  public Dictionary<string, int> StatisticChanges { get; set; } = [];
  public Dictionary<string, int> StatusConditions { get; set; } = [];

  public Modification<UrlUnit>? Reference { get; set; }
  public Modification<NotesUnit>? Notes { get; set; }

  public bool HasChanges => UniqueName != null || DisplayName != null || Description != null
    || Accuracy != null || Power != null || PowerPoints != null
    || StatisticChanges.Count > 0 || StatusConditions.Count > 0
    || Reference != null || Notes != null;
}
