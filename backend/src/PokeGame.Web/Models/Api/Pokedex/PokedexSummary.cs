using PokeGame.Application.Pokedex.Models;

namespace PokeGame.Web.Models.Api.Pokedex
{
  public class PokedexSummary
  {
    public PokedexSummary(PokedexModel model)
    {
      Species = model.Species == null ? null : new PokedexSpecies(model.Species);

      HasCaught = model.HasCaught;
      UpdatedAt = model.UpdatedAt;
    }

    public PokedexSpecies? Species { get; set; }

    public bool HasCaught { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
