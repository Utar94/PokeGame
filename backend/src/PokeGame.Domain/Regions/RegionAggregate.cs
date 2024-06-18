using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using PokeGame.Domain.Regions.Events;

namespace PokeGame.Domain.Regions;

public class RegionAggregate : AggregateRoot
{
  public static readonly IUniqueNameSettings UniqueNameSettings = new ReadOnlyUniqueNameSettings("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_");

  private RegionUpdatedEvent _updatedEvent = new();

  public new RegionId Id => new(base.Id);

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
        _updatedEvent.DisplayName = new Modification<DisplayNameUnit>(value);
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
        _updatedEvent.Description = new Modification<DescriptionUnit>(value);
      }
    }
  }

  private UrlUnit? _reference = null;
  public UrlUnit? Reference
  {
    get => _reference;
    set
    {
      if (value != _reference)
      {
        _reference = value;
        _updatedEvent.Reference = new Modification<UrlUnit>(value);
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
        _updatedEvent.Notes = new Modification<NotesUnit>(value);
      }
    }
  }

  public RegionAggregate() : base()
  {
  }

  public RegionAggregate(UniqueNameUnit uniqueName, ActorId actorId = default, RegionId? id = null)
    : base((id ?? RegionId.NewId()).AggregateId)
  {
    Raise(new RegionCreatedEvent(uniqueName), actorId);
  }
  protected virtual void Apply(RegionCreatedEvent @event)
  {
    _uniqueName = @event.UniqueName;
  }

  public void Delete(ActorId actorId = default)
  {
    if (!IsDeleted)
    {
      Raise(new RegionDeletedEvent(), actorId);
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
  protected virtual void Apply(RegionUpdatedEvent @event)
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
