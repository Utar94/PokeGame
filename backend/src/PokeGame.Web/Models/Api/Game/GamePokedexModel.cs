using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain;

namespace PokeGame.Web.Models.Api.Game
{
  public class GamePokedexModel
  {
    public GamePokedexModel(IEnumerable<PokedexModel> entries, bool hasNational = false, Region? region = null)
    {
      Entries = entries?.Select(entry => new GamePokedexEntryModel(entry, region)) ?? throw new ArgumentNullException(nameof(entries));
      HasNational = hasNational;
    }

    public IEnumerable<GamePokedexEntryModel> Entries { get; set; }
    public bool HasNational { get; set; }
  }
}
