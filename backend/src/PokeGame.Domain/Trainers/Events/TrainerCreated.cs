using MediatR;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Domain.Trainers.Events
{
  public class TrainerCreated : DomainEvent, INotification
  {
    public TrainerCreated(CreateTrainerPayload payload)
    {
      Payload = payload;
    }

    public CreateTrainerPayload Payload { get; private set; }
  }
}
