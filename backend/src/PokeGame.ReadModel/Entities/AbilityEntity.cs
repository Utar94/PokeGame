using PokeGame.Domain.Abilities;

namespace PokeGame.ReadModel.Entities
{
  internal class AbilityEntity : Entity
  {
    public AbilityType? Type { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Reference { get; private set; }

    public List<PokemonEntity> Pokemon { get; private set; } = new();
    public List<SpeciesAbilityEntity> SpeciesAbilities { get; private set; } = new();

    public void Synchronize(Ability ability)
    {
      base.Synchronize(ability);

      Type = ability.Type;

      Name = ability.Name;
      Description = ability.Description;

      Notes = ability.Notes;
      Reference = ability.Reference;
    }
  }
}
