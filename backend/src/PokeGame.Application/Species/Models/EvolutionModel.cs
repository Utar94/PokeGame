using PokeGame.Application.Items.Models;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species;

namespace PokeGame.Application.Species.Models
{
  public class EvolutionModel
  {
    public SpeciesModel? Species { get; set; }

    public EvolutionMethod Method { get; set; }

    public PokemonGender? Gender { get; set; }
    public bool HighFriendship { get; set; }
    public ItemModel? Item { get; set; }
    public byte Level { get; set; }
    public string? Location { get; set; }
    public MoveModel? Move { get; set; }
    public Region? Region { get; set; }
    public TimeOfDay? TimeOfDay { get; set; }

    public string? Notes { get; set; }
  }
}
