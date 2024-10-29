using PokeGame.Domain.Regions;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class RegionEntity : AggregateEntity
{
  public int RegionId { get; private set; }
  public Guid Id { get; private set; }

  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }

  public string? Link { get; private set; }
  public string? Notes { get; private set; }

  public RegionEntity(Region.CreatedEvent @event) : base(@event)
  {
    Id = @event.AggregateId.ToGuid();

    Name = @event.Name.Value;
  }

  private RegionEntity() : base()
  {
  }

  public void Update(Region.UpdatedEvent @event)
  {
    base.Update(@event);

    if (@event.Name != null)
    {
      Name = @event.Name.Value;
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

  public override string ToString() => $"{Name} | {base.ToString()}";
}
