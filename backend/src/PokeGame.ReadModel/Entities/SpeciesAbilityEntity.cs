namespace PokeGame.ReadModel.Entities
{
  internal class SpeciesAbilityEntity
  {
    public SpeciesAbilityEntity(SpeciesEntity species, AbilityEntity ability)
    {
      Species = species;
      SpeciesId = species.Sid;
      Ability = ability;
      AbilityId = ability.Sid;
    }
    private SpeciesAbilityEntity()
    {
    }

    public SpeciesEntity? Species { get; private set; }
    public int SpeciesId { get; private set; }

    public AbilityEntity? Ability { get; private set; }
    public int AbilityId { get; private set; }
  }
}
