using MediatR;
using PokeGame.Application.Pokemon.Models;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class SwapPokemonMutation : IRequest<PokemonModel>
  {
    public SwapPokemonMutation(Guid id, Guid otherId)
    {
      Id = id;
      OtherId = otherId;
    }

    public Guid Id { get; }
    public Guid OtherId { get; }
  }
}
