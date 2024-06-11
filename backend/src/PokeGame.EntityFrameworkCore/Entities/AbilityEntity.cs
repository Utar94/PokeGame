using PokeGame.Domain.Abilities.Events;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class AbilityEntity : AggregateEntity
{
  public int AbilityId { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => PokemonDb.Normalize(UniqueName);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public string? Reference { get; private set; }
  public string? Notes { get; private set; }

  public AbilityEntity(AbilityCreatedEvent @event) : base(@event)
  {
    UniqueName = @event.UniqueName.Value;
  }

  private AbilityEntity() : base()
  {
  }

  public void Update(AbilityUpdatedEvent @event)
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

    if (@event.Reference != null)
    {
      Reference = @event.Reference.Value?.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value?.Value;
    }
  }
}
