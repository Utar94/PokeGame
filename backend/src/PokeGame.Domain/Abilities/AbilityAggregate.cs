using Logitar.EventSourcing;
using Logitar.Identity.Contracts.Settings;
using PokeGame.Contracts;
using PokeGame.Domain.Abilities.Events;

namespace PokeGame.Domain.Abilities;

public class AbilityAggregate : AggregateRoot
{
  public static readonly IUniqueNameSettings UniqueNameSettings = new ReadOnlyUniqueNameSettings("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_");

  private AbilityUpdatedEvent _updatedEvent = new();

  public new AbilityId Id => new(base.Id);

  private UniqueNameUnit? _uniqueName = null;
  public UniqueNameUnit UniqueName
  {
    get => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");
    set
    {
      if (value != _uniqueName)
      {
        _uniqueName = value;
        _updatedEvent.UniqueName = value;
      }
    }
  }
  private DisplayNameUnit? _displayName = null;
  public DisplayNameUnit? DisplayName
  {
    get => _displayName;
    set
    {
      if (value != _displayName)
      {
        _displayName = value;
        _updatedEvent.DisplayName = new Change<DisplayNameUnit>(value);
      }
    }
  }
  private DescriptionUnit? _description = null;
  public DescriptionUnit? Description
  {
    get => _description;
    set
    {
      if (value != _description)
      {
        _description = value;
        _updatedEvent.Description = new Change<DescriptionUnit>(value);
      }
    }
  }

  private ReferenceUnit? _reference = null;
  public ReferenceUnit? Reference
  {
    get => _reference;
    set
    {
      if (value != _reference)
      {
        _reference = value;
        _updatedEvent.Reference = new Change<ReferenceUnit>(value);
      }
    }
  }
  private NotesUnit? _notes = null;
  public NotesUnit? Notes
  {
    get => _notes;
    set
    {
      if (value != _notes)
      {
        _notes = value;
        _updatedEvent.Notes = new Change<NotesUnit>(value);
      }
    }
  }

  public AbilityAggregate() : base()
  {
  }

  public AbilityAggregate(UniqueNameUnit uniqueName, ActorId actorId = default, AbilityId? id = null)
    : base((id ?? AbilityId.NewId()).AggregateId)
  {
    Raise(new AbilityCreatedEvent(uniqueName), actorId);
  }
  protected virtual void Apply(AbilityCreatedEvent @event)
  {
    _uniqueName = @event.UniqueName;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new AbilityDeletedEvent(), actorId);
    }
  }

  public void Update(ActorId actorId = default)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, actorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(AbilityUpdatedEvent @event)
  {
    if (@event.UniqueName != null)
    {
      _uniqueName = @event.UniqueName;
    }
    if (@event.DisplayName != null)
    {
      _displayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      _description = @event.Description.Value;
    }

    if (@event.Reference != null)
    {
      _reference = @event.Reference.Value;
    }
    if (@event.Notes != null)
    {
      _notes = @event.Notes.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";
}
