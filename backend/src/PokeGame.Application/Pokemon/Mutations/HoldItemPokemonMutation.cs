using MediatR;
using PokeGame.Application.Pokemon.Models;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class HoldItemPokemonMutation : IRequest<PokemonModel>
  {
    public HoldItemPokemonMutation(Guid id, Guid? itemId = null)
    {
      Id = id;
      ItemId = itemId;
    }

    public Guid Id { get; }
    public Guid? ItemId { get; }
  }
}
