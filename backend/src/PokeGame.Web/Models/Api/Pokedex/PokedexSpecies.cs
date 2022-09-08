using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Web.Models.Api.Pokedex
{
  public class PokedexSpecies
  {
    public PokedexSpecies(SpeciesModel species)
    {
      Number = species.Number;

      PrimaryType = species.PrimaryType;
      SecondaryType = species.SecondaryType;

      Name = species.Name;
      Category = species.Category;
      Description = species.Description;

      GenderRatio = species.GenderRatio;
      Height = species.Height;
      Weight = species.Weight;
    }

    public int Number { get; set; }

    public PokemonType PrimaryType { get; set; }
    public PokemonType? SecondaryType { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }

    public double? GenderRatio { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
  }
}
