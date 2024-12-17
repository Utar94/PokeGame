using PokeGame.Domain.Speciez.Events;

namespace PokeGame.Infrastructure.Entities;

internal class RegionalSpeciesEntity
{
  public SpeciesEntity? Species { get; private set; }
  public int SpeciesId { get; private set; }

  public RegionEntity? Region { get; private set; }
  public int RegionId { get; private set; }

  public int Number { get; private set; }

  public RegionalSpeciesEntity(SpeciesEntity species, RegionEntity region, SpeciesRegionalNumberChanged @event)
  {
    Species = species;
    SpeciesId = species.SpeciesId;

    Region = region;
    RegionId = region.RegionId;

    Update(@event);
  }

  private RegionalSpeciesEntity()
  {
  }

  public void Update(SpeciesRegionalNumberChanged @event)
  {
    Number = @event.Number ?? throw new ArgumentException($"The {nameof(@event.Number)} is required.", nameof(@event));
  }
}
