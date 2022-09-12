using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Web.Models.Api.Species
{
  public class SpeciesSummary : AggregateSummary
  {
    public SpeciesSummary(SpeciesModel model) : base(model)
    {
      Number = model.Number;
      PrimaryType = model.PrimaryType;
      SecondaryType = model.SecondaryType;
      Name = model.Name;
      Category = model.Category;
      CatchRate = model.CatchRate;
    }

    public int Number { get; set; }

    public PokemonType PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }

    public byte? CatchRate { get; set; }
  }
}
