using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class UsePokemonMoveMutation : IRequest<IEnumerable<PokemonModel>>
  {
    public UsePokemonMoveMutation(Guid id, Guid moveId, UsePokemonMovePayload payload)
    {
      Id = id;
      MoveId = moveId;
      Payload = payload;
    }

    public Guid Id { get; }
    public Guid MoveId { get; }
    public UsePokemonMovePayload Payload { get; }
  }
}
