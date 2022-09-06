using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonCreated : DomainEvent
  {
    public PokemonCreated(CreatePokemonPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreatePokemonPayload Payload { get; private set; }
  }
}
