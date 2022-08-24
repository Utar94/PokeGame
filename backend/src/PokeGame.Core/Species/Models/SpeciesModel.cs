using PokeGame.Core.Abilities.Models;
using PokeGame.Core.Models;

namespace PokeGame.Core.Species.Models
{
  public class SpeciesModel : AggregateModel
  {
    public int Number { get; set; }

    public PokemonType PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public AbilityModel? Ability { get; set; }

    public string Name { get; set; } = null!;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
