using PokeGame.Application.Pokedex.Models;
using PokeGame.Application.Regions.Models;

namespace PokeGame.Web.Models.Api.Game
{
  public class GamePokedexModel
  {
    public GamePokedexModel(IEnumerable<PokedexModel> entries, bool hasNational = false, RegionModel? region = null)
    {
      Entries = entries.Select(entry => new GamePokedexEntryModel(entry, region));
      HasNational = hasNational;
    }

    public IEnumerable<GamePokedexEntryModel> Entries { get; set; }
    public bool HasNational { get; set; }
  }
}
