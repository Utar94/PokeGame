using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Species.Queries
{
  public class GetSpeciesListQuery : IRequest<ListModel<SpeciesModel>>
  {
    public string? Search { get; set; }
    public PokemonType? Type { get; set; }

    public SpeciesSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
