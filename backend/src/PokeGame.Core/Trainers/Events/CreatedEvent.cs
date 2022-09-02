using PokeGame.Core.Trainers.Payloads;

namespace PokeGame.Core.Trainers.Events
{
  public class CreatedEvent : CreatedEventBase
  {
    public CreatedEvent(CreateTrainerPayload payload, Guid userId) : base(userId)
    {
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public CreateTrainerPayload Payload { get; private set; }
  }
}
