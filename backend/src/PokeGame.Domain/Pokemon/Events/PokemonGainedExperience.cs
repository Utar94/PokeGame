using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonGainedExperience : DomainEvent, INotification
  {
    public PokemonGainedExperience(ExperienceGainPayload payload)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public ExperienceGainPayload Payload { get; private set; }
  }
}
