using PokeGame.Application.Abilities.Models;

namespace PokeGame.Web.Models.Api.Game
{
  public class AbilitySummary
  {
    public AbilitySummary(AbilityModel ability)
    {
      Name = ability.Name;
      Description = ability.Description;
    }

    public string Name { get; set; }
    public string? Description { get; set; }
  }
}
