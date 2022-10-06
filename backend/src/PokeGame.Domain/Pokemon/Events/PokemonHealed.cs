using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonHealed : DomainEvent, INotification
  {
    public PokemonHealed(HealPokemonPayload payload, Dictionary<Guid, byte>? movePowerPoints = null)
    {
      MovePowerPoints = movePowerPoints;
      Payload = payload;
    }

    public Dictionary<Guid, byte>? MovePowerPoints { get; private set; }
    public HealPokemonPayload Payload { get; private set; }
  }
}
