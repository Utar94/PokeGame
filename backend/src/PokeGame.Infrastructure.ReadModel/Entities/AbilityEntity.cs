using PokeGame.Domain.Abilities;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class AbilityEntity : Entity
  {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public List<PokemonEntity> Pokemon { get; set; } = new();
    public List<SpeciesAbilityEntity> SpeciesAbilities { get; set; } = new();

    public void Synchronize(Ability ability)
    {
      base.Synchronize(ability);

      Name = ability.Name;
      Description = ability.Description;

      Notes = ability.Notes;
      Reference = ability.Reference;
    }
  }
}
