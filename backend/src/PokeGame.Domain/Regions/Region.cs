using Logitar.EventSourcing;
using PokeGame.Domain.Regions.Events;

namespace PokeGame.Domain.Regions;

public class Region : AggregateRoot
{
  public new RegionId Id => new(base.Id);

  private UniqueName? _uniqueName = null;
  public UniqueName UniqueName => _uniqueName ?? throw new InvalidOperationException($"The {nameof(UniqueName)} has not been initialized yet.");
  public DisplayName? DisplayName { get; private set; }
  public Description? Description { get; private set; }

  public Url? Link { get; private set; }
  public Notes? Notes { get; private set; }

  public Region() : base()
  {
  }

  public Region(UniqueName uniqueName, UserId userId, RegionId? id = null) : base((id ?? RegionId.NewId()).AggregateId)
  {
    Raise(new RegionCreated(uniqueName), userId.ActorId);
  }
  protected virtual void Apply(RegionCreated @event)
  {
    _uniqueName = @event.UniqueName;
  }

  public void Delete(UserId userId)
  {
    if (!IsDeleted)
    {
      Raise(new RegionDeleted(), userId.ActorId);
    }
  }

  public void Update(RegionUpdates updates, UserId userId)
  {
    RegionUpdated @event = new(
      updates.UniqueName != null && updates.UniqueName != UniqueName ? updates.UniqueName : null,
      updates.DisplayName != null && updates.DisplayName.Value != DisplayName ? updates.DisplayName : null,
      updates.Description != null && updates.Description.Value != Description ? updates.Description : null,
      updates.Link != null && updates.Link.Value != Link ? updates.Link : null,
      updates.Notes != null && updates.Notes.Value != Notes ? updates.Notes : null);
    if (@event.UniqueName != null || @event.DisplayName != null || @event.Description != null || @event.Link != null || @event.Notes != null)
    {
      Raise(@event, userId.ActorId);
    }
  }
  protected virtual void Apply(RegionUpdated @event)
  {
    if (@event.UniqueName != null)
    {
      _uniqueName = @event.UniqueName;
    }
    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value;
    }

    if (@event.Link != null)
    {
      Link = @event.Link.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value;
    }
  }

  public override string ToString() => $"{DisplayName?.ToString() ?? UniqueName.ToString()} | {base.ToString()}";
}
