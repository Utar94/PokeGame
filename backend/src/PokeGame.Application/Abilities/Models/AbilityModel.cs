using PokeGame.Application.Models;

namespace PokeGame.Application.Abilities.Models
{
  public class AbilityModel : AggregateModel
  {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
