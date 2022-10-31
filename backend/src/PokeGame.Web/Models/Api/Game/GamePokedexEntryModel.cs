using PokeGame.Application.Pokedex.Models;
using PokeGame.Application.Regions.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Web.Models.Api.Game
{
  public class GamePokedexEntryModel
  {
    public GamePokedexEntryModel(PokedexModel model, RegionModel? region = null)
    {
      HasCaught = model.HasCaught;

      SpeciesModel species = model.Species ?? throw new ArgumentException($"The {nameof(model.Species)} is required.", nameof(model));

      Number = region == null ? species.Number : species.RegionalNumbers.Single(x => x.Region?.Id == region.Id).Number;

      Name = species.Name;

      GenderRatio = species.GenderRatio;

      Picture = species.Picture;

      if (model.HasCaught)
      {
        PrimaryType = species.PrimaryType;
        SecondaryType = species.SecondaryType;

        Category = species.Category;
        Description = species.Description;

        Height = species.Height;
        Weight = species.Weight;
      }
    }

    public bool HasCaught { get; set; }

    public int Number { get; set; }

    public PokemonType? PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public double? GenderRatio { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }

    public string? Picture { get; set; }
  }
}
