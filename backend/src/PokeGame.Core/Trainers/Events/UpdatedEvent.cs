using PokeGame.Core.Trainers.Payloads;

namespace PokeGame.Core.Trainers.Events
{
  public class UpdatedEvent : UpdatedEventBase
  {
    public UpdatedEvent(UpdateTrainerPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public UpdateTrainerPayload Payload { get; private set; }
  }
}
