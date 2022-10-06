using MediatR;
using PokeGame.Domain.Pokemon.Payloads;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonGainedExperience : DomainEvent, INotification
  {
    public PokemonGainedExperience(ExperienceGainPayload payload, bool hasBeenCaughtWithLuxuryBall, bool isHoldingSootheBell)
    {
      HasBeenCaughtWithLuxuryBall = hasBeenCaughtWithLuxuryBall;
      IsHoldingSootheBell = isHoldingSootheBell;
      Payload = payload;
    }

    public bool HasBeenCaughtWithLuxuryBall { get; private set; }
    public bool IsHoldingSootheBell { get; private set; }
    public ExperienceGainPayload Payload { get; private set; }
  }
}
