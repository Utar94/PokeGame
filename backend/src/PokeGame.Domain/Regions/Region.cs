using PokeGame.Domain.Regions.Events;
using PokeGame.Domain.Regions.Payloads;

namespace PokeGame.Domain.Regions
{
  public class Region : Aggregate
  {
    public Region(CreateRegionPayload payload)
    {
      ApplyChange(new RegionCreated(payload));
    }
    private Region()
    {
    }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public void Delete() => ApplyChange(new RegionDeleted());
    public void Update(UpdateRegionPayload payload) => ApplyChange(new RegionUpdated(payload));

    protected virtual void Apply(RegionCreated @event)
    {
      Apply(@event.Payload);
    }
    protected virtual void Apply(RegionDeleted @event)
    {
      Delete(@event);
    }
    protected virtual void Apply(RegionUpdated @event)
    {
      Apply(@event.Payload);
    }

    private void Apply(SaveRegionPayload payload)
    {
      Name = payload.Name.Trim();
      Description = payload.Description?.CleanTrim();

      Notes = payload.Notes?.CleanTrim();
      Reference = payload.Reference;
    }

    public override string ToString() => $"{Name} | {base.ToString()}";
  }
}
