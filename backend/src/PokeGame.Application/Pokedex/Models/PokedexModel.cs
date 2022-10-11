using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Pokedex.Models
{
  public class PokedexModel
  {
    public SpeciesModel? Species { get; set; }

    public bool HasCaught { get; set; }
    public DateTime UpdatedOn { get; set; }
  }
}
