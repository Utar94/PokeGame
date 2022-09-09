using PokeGame.Application.Abilities.Models;

namespace PokeGame.Web.Models.Api.Ability
{
  public class AbilitySummary : AggregateSummary
  {
    public AbilitySummary(AbilityModel model) : base(model)
    {
      Name = model.Name;
      Description = model.Description;

      Notes = model.Notes;
      Reference = model.Reference;
    }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
