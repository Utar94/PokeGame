using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Application.Pokemon.Queries
{
  public class GetPokemonListQuery : IRequest<ListModel<PokemonModel>>
  {
    public PokemonGender? Gender { get; set; }
    public byte? InBox { get; set; }
    public bool? InParty { get; set; }
    public bool? IsWild { get; set; }
    public string? Search { get; set; }
    public Guid? SpeciesId { get; set; }
    public Guid? TrainerId { get; set; }

    public PokemonSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
