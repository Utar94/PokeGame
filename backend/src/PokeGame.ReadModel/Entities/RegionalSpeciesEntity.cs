namespace PokeGame.ReadModel.Entities
{
  internal class RegionalSpeciesEntity
  {
    public RegionalSpeciesEntity(SpeciesEntity species, RegionEntity region)
    {
      Species = species;
      SpeciesId = species.Sid;
      Region = region;
      RegionId = region.Sid;
    }
    private RegionalSpeciesEntity()
    {
    }

    public SpeciesEntity? Species { get; private set; }
    public int SpeciesId { get; private set; }

    public RegionEntity? Region { get; private set; }
    public int RegionId { get; private set; }

    public int Number { get; set; }
  }
}
