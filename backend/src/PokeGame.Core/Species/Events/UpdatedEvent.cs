using PokeGame.Core.Species.Payloads;

namespace PokeGame.Core.Species.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateSpeciesPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateSpeciesPayload Payload { get; private set; }
  }
}
