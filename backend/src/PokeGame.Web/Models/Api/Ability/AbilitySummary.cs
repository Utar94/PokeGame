using PokeGame.Application.Abilities.Models;

namespace PokeGame.Web.Models.Api.Ability
{
  public class AbilitySummary : AggregateSummary
  {
    public AbilitySummary(AbilityModel model) : base(model)
    {
      Name = model.Name;
    }

    public string Name { get; set; } = string.Empty;
  }
}
