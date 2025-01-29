using Logitar.EventSourcing;
using PokeGame.Domain.Abilities.Events;

namespace PokeGame.Domain.Abilities;

public class Ability : AggregateRoot
{
  private AbilityUpdated _updated = new();

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
        _updated.UniqueName = value;
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
        _updated.DisplayName = new Change<DisplayName>(value);
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
        _updated.Description = new Change<Description>(value);
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
        _updated.Link = new Change<Url>(value);
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
        _updated.Notes = new Change<Notes>(value);
      }
    }
  }

  public Ability() : base()
  {
  }

  public Ability(UniqueName uniqueName, ActorId? actorId = null, AbilityId? id = null) : base((id ?? AbilityId.NewId()).StreamId)
  {
    Raise(new AbilityCreated(uniqueName), actorId);
  }
  protected virtual void Handle(AbilityCreated @event)
  {
    _uniqueName = @event.UniqueName;
  }

  public void Delete(ActorId? actorId = null)
  {
    if (!IsDeleted)
    {
      Raise(new AbilityDeleted(), actorId);
    }
  }

  public void Update(ActorId? actorId = null)
  {
    if (_updated.HasChanges)
    {
      Raise(_updated, actorId, DateTime.Now);
      _updated = new();
    }
  }
  protected virtual void Handle(AbilityUpdated @event)
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
}
