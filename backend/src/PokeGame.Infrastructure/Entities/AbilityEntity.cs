using PokeGame.Domain.Abilities.Events;
using PokeGame.Infrastructure.PokeGameDb;

namespace PokeGame.Infrastructure.Entities;

internal class AbilityEntity : AggregateEntity
{
  public int AbilityId { get; private set; }
  public Guid Id { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => Helper.Normalize(UniqueName);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public string? Link { get; private set; }
  public string? Notes { get; private set; }

  public AbilityEntity(AbilityCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    UniqueName = @event.UniqueName.Value;
  }

  private AbilityEntity() : base()
  {
  }

  public void Update(AbilityUpdated @event)
  {
    base.Update(@event);

    if (@event.UniqueName != null)
    {
      UniqueName = @event.UniqueName.Value;
    }
    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }

    if (@event.Link != null)
    {
      Link = @event.Link.Value?.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value?.Value;
    }
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
