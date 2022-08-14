using PokeGame.Core.Models;

namespace PokeGame.Core.Abilities.Models
{
  public class AbilityModel : AggregateModel
  {
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
