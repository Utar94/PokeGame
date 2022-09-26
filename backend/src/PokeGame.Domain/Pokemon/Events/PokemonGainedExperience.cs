using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonGainedExperience : DomainEvent, INotification
  {
    public PokemonGainedExperience(ExperienceGainPayload payload, bool isHoldingSootheBell)
    {
      IsHoldingSootheBell = isHoldingSootheBell;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public bool IsHoldingSootheBell { get; private set; }
    public ExperienceGainPayload Payload { get; private set; }
  }
}
