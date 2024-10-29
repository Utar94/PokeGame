using Logitar.EventSourcing;
using MediatR;
using PokeGame.Contracts;

namespace PokeGame.Domain.Regions;

public class Region : AggregateRoot
{
  private UpdatedEvent _updatedEvent = new();

  public new RegionId Id => new(base.Id);

  private Name? _name = null;
  public Name Name
  {
    get => _name ?? throw new InvalidOperationException($"The {nameof(Name)} has not been initialized yet.");
    set
    {
      if (_name != value)
      {
        _name = value;
        _updatedEvent.Name = value;
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

  public Region() : base()
  {
  }

  public Region(Name name, UserId userId, RegionId? id = null) : base((id ?? RegionId.NewId()).AggregateId)
  {
    Raise(new CreatedEvent(name), userId.ActorId);
  }
  protected virtual void Apply(CreatedEvent @event)
  {
    _name = @event.Name;
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
    if (@event.Name != null)
    {
      _name = @event.Name;
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

  public override string ToString() => $"{Name} | {base.ToString()}";

  public class CreatedEvent : DomainEvent, INotification
  {
    public Name Name { get; }

    public CreatedEvent(Name name)
    {
      Name = name;
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
    public Name? Name { get; set; }
    public Change<Description>? Description { get; set; }

    public Change<Url>? Link { get; set; }
    public Change<Notes>? Notes { get; set; }

    public bool HasChanges => Name != null || Description != null || Link != null || Notes != null;
  }
}
