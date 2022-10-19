using MediatR;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Domain.Trainers.Events
{
  public class TrainerUpdated : DomainEvent, INotification
  {
    public TrainerUpdated(UpdateTrainerPayload payload)
    {
      Payload = payload;
    }

    public UpdateTrainerPayload Payload { get; private set; }
  }
}
