using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Web.Models.Api.Game
{
  internal static class PokemonExtensions
  {
    public static string? GetPicture(this PokemonModel pokemon)
    {
      SpeciesModel? species = pokemon.Species;
      if (species == null)
      {
        return null;
      }

      if (pokemon.IsShiny)
      {
        return pokemon.Gender == PokemonGender.Female
          ? (species.PictureShinyFemale ?? species.PictureShiny ?? species.PictureFemale ?? species.Picture)
          : (species.PictureShiny ?? species.Picture);
      }

      return pokemon.Gender == PokemonGender.Female
        ? (species.PictureFemale ?? species.Picture)
        : species.Picture;
    }
  }
}
