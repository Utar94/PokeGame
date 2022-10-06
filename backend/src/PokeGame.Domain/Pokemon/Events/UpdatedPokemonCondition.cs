using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class UpdatedPokemonCondition : DomainEvent, INotification
  {
    public UpdatedPokemonCondition(UpdatePokemonConditionPayload payload)
    {
      Payload = payload;
    }

    public UpdatePokemonConditionPayload Payload { get; private set; }
  }
}
