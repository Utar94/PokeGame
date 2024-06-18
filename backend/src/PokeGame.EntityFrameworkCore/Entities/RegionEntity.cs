using PokeGame.Domain.Regions.Events;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class RegionEntity : AggregateEntity
{
  public int RegionId { get; private set; }

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

  public RegionEntity(RegionCreatedEvent @event) : base(@event)
  {
    UniqueName = @event.UniqueName.Value;
  }

  private RegionEntity() : base()
  {
  }

  public void Update(RegionUpdatedEvent @event)
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
