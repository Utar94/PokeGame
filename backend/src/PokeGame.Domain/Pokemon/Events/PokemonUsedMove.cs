using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonUsedMove : DomainEvent, INotification
  {
    public PokemonUsedMove(Guid moveId, UsePokemonMovePayload payload)
    {
      MoveId = moveId;
      Payload = payload;
    }

    public Guid MoveId { get; private set; }
    public UsePokemonMovePayload Payload { get; private set; }
  }
}
