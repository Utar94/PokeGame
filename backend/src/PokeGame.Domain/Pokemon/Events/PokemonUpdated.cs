using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonUpdated : DomainEvent, INotification
  {
    public PokemonUpdated(UpdatePokemonPayload payload)
    {
      Payload = payload;
    }

    public UpdatePokemonPayload Payload { get; private set; }
  }
}
