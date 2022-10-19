using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Pokedex.Queries
{
  public class GetPokedexEntriesQuery : IRequest<ListModel<PokedexModel>>
  {
    public bool? HasCaught { get; set; }
    public Region? Region { get; set; }
    public string? Search { get; set; }
    public Guid TrainerId { get; set; }
    public PokemonType? Type { get; set; }

    public PokedexSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
