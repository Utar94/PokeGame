using MediatR;
using PokeGame.Application.Pokemon.Models;

namespace PokeGame.Application.Pokemon.Queries
{
  public class GetPokemonQuery : IRequest<PokemonModel?>
  {
    public GetPokemonQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
