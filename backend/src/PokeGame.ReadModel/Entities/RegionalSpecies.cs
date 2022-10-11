using PokeGame.Domain;

namespace PokeGame.ReadModel.Entities
{
  internal class RegionalSpeciesEntity
  {
    public RegionalSpeciesEntity(SpeciesEntity species, Region region)
    {
      Species = species;
      SpeciesId = species.Sid;
      Region = region;
    }
    private RegionalSpeciesEntity()
    {
    }

    public SpeciesEntity? Species { get; private set; }
    public int SpeciesId { get; private set; }

    public Region Region { get; private set; }

    public int Number { get; set; }
  }
}
