using PokeGame.Domain.Regions;

namespace PokeGame.ReadModel.Entities
{
  internal class RegionEntity : Entity
  {
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public List<TrainerEntity> Trainers { get; private set; } = new();

    public void Synchronize(Region region)
    {
      base.Synchronize(region);

      Name = region.Name;
      Description = region.Description;

      Notes = region.Notes;
      Reference = region.Reference;
    }
  }
}
