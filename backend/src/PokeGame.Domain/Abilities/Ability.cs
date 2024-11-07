using Logitar.EventSourcing;
using MediatR;
using PokeGame.Contracts;

namespace PokeGame.Domain.Abilities;

public class Ability : AggregateRoot
{
  private UpdatedEvent _updatedEvent = new();

  public new AbilityId Id => new(base.Id);

  private UniqueName? _uniqueName = null;
  public UniqueName UniqueName
  {
    get => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");
    set
    {
      if (_uniqueName != value)
      {
        _uniqueName = value;
        _updatedEvent.UniqueName = value;
      }
    }
  }
  private DisplayName? _displayName = null;
  public DisplayName? DisplayName
  {
    get => _displayName;
    set
    {
      if (_displayName != value)
      {
        _displayName = value;
        _updatedEvent.DisplayName = new Change<DisplayName>(value);
      }
    }
  }
  private Description? _description = null;
  public Description? Description
  {
    get => _description;
    set
    {
      if (_description != value)
      {
        _description = value;
        _updatedEvent.Description = new Change<Description>(value);
      }
    }
  }

  private Url? _link = null;
  public Url? Link
  {
    get => _link;
    set
    {
      if (_link != value)
      {
        _link = value;
        _updatedEvent.Link = new Change<Url>(value);
      }
    }
  }
  private Notes? _notes = null;
  public Notes? Notes
  {
    get => _notes;
    set
    {
      if (_notes != value)
      {
        _notes = value;
        _updatedEvent.Notes = new Change<Notes>(value);
      }
    }
  }

  public Ability() : base()
  {
  }

  public Ability(UniqueName uniqueName, UserId userId, AbilityId? id = null) : base((id ?? AbilityId.NewId()).AggregateId)
  {
    Raise(new CreatedEvent(uniqueName), userId.ActorId);
  }
  protected virtual void Apply(CreatedEvent @event)
  {
    _uniqueName = @event.UniqueName;
  }

  public void Delete(UserId userId)
  {
    if (!IsDeleted)
    {
      Raise(new DeletedEvent(), userId.ActorId);
    }
  }

  public void Update(UserId userId)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, userId.ActorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(UpdatedEvent @event)
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

    if (@event.Link != null)
    {
      _link = @event.Link.Value;
    }
    if (@event.Notes != null)
    {
      _notes = @event.Notes.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.Value ?? UniqueName.Value} | {base.ToString()}";

  public class CreatedEvent : DomainEvent, INotification
  {
    public UniqueName UniqueName { get; }

    public CreatedEvent(UniqueName uniqueName)
    {
      UniqueName = uniqueName;
    }
  }

  public class DeletedEvent : DomainEvent, INotification
  {
    public DeletedEvent()
    {
      IsDeleted = true;
    }
  }

  public class UpdatedEvent : DomainEvent, INotification
  {
    public UniqueName? UniqueName { get; set; }
    public Change<DisplayName>? DisplayName { get; set; }
    public Change<Description>? Description { get; set; }

    public Change<Url>? Link { get; set; }
    public Change<Notes>? Notes { get; set; }

    public bool HasChanges => UniqueName != null || DisplayName != null || Description != null || Link != null || Notes != null;
  }
}
