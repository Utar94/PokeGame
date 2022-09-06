namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class Ability : Entity
  {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public List<Species> Species { get; set; } = new();

    public void Synchronize(Domain.Abilities.Ability ability)
    {
      base.Synchronize(ability);

      Name = ability.Name;
      Description = ability.Description;

      Notes = ability.Notes;
      Reference = ability.Reference;
    }
  }
}
