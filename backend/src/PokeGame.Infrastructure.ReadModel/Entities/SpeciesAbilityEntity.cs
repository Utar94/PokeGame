namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class SpeciesAbilityEntity
  {
    public SpeciesEntity? Species { get; set; }
    public int SpeciesId { get; set; }

    public AbilityEntity? Ability { get; set; }
    public int AbilityId { get; set; }
  }
}
