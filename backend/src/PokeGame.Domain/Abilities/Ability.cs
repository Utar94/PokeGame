using Logitar.EventSourcing;
using MediatR;
using PokeGame.Contracts;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Domain.Abilities;

public class Ability : AggregateRoot
{
  private UpdatedEvent _updatedEvent = new();

  public new AbilityId Id => new(base.Id);

  private AbilityKind? _kind = null;
  public AbilityKind? Kind
  {
    get => _kind;
    set
    {
      if (_kind != value)
      {
        if (value.HasValue && !Enum.IsDefined(value.Value))
        {
          throw new ArgumentOutOfRangeException(nameof(Kind));
        }

        _kind = value;
        _updatedEvent.Kind = new Change<AbilityKind?>(value);
      }
    }
  }

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

  public Ability() : base()
  {
  }

  public Ability(Name name, ActorId actorId, AbilityId? id = null) : base((id ?? AbilityId.NewId()).AggregateId)
  {
    Raise(new CreatedEvent(name), actorId);
  }
  protected virtual void Apply(CreatedEvent @event)
  {
    _name = @event.Name;
  }

  public void Delete(ActorId actorId)
  {
    if (!IsDeleted)
    {
      Raise(new DeletedEvent(), actorId);
    }
  }

  public void Update(ActorId actorId)
  {
    if (_updatedEvent.HasChanges)
    {
      Raise(_updatedEvent, actorId, DateTime.Now);
      _updatedEvent = new();
    }
  }
  protected virtual void Apply(UpdatedEvent @event)
  {
    if (@event.Kind != null)
    {
      _kind = @event.Kind.Value;
    }

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
    public Change<AbilityKind?>? Kind { get; set; }

    public Name? Name { get; set; }
    public Change<Description>? Description { get; set; }

    public Change<Url>? Link { get; set; }
    public Change<Notes>? Notes { get; set; }

    public bool HasChanges => Kind != null || Name != null || Description != null || Link != null || Notes != null;
  }
}
