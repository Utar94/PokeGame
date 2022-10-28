using PokeGame.Application.Models;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Models
{
  public class AbilityModel : AggregateModel
  {
    public AbilityKind? Kind { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
