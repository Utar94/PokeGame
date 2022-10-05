using PokeGame.Application.Pokedex.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Web.Models.Api.Game
{
  public class GamePokedexEntryModel
  {
    public GamePokedexEntryModel(PokedexModel model, Region? region = null)
    {
      ArgumentNullException.ThrowIfNull(model);

      HasCaught = model.HasCaught;

      SpeciesModel species = model.Species ?? throw new ArgumentException($"The {nameof(model.Species)} is required.", nameof(model));

      Number = region.HasValue
        ? species.RegionalNumbers.Single(x => x.Region == region.Value).Number
        : species.Number;

      Name = species.Name;

      GenderRatio = species.GenderRatio;

      Picture = species.Picture;

      if (model.HasCaught)
      {
        Types = string.Join(", ", new[] { species.PrimaryType, species.SecondaryType }.Where(x => x != null));

        Category = species.Category;
        Description = species.Description;

        Height = species.Height;
        Weight = species.Weight;
      }
    }

    public bool HasCaught { get; set; }

    public int Number { get; set; }

    public string? Types { get; set; } // TODO(fpion): refactor

    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public double? GenderRatio { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }

    public string? Picture { get; set; }
  }
}
