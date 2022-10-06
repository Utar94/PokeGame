using MediatR;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Application.Trainers.Mutations
{
  public class CreateTrainerMutation : IRequest<TrainerModel>
  {
    public CreateTrainerMutation(CreateTrainerPayload payload)
    {
      Payload = payload;
    }

    public CreateTrainerPayload Payload { get; }
  }
}
