using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonHealed : DomainEvent, INotification
  {
    public PokemonHealed(HealPokemonPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public HealPokemonPayload Payload { get; private set; }
  }
}
