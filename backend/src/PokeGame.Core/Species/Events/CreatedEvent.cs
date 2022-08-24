using PokeGame.Core.Species.Payloads;

namespace PokeGame.Core.Species.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateSpeciesPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateSpeciesPayload Payload { get; private set; }
  }
}
