using MediatR;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Application.Trainers.Mutations
{
  public class UpdateTrainerMutation : IRequest<TrainerModel>
  {
    public UpdateTrainerMutation(Guid id, UpdateTrainerPayload payload)
    {
      Id = id;
      Payload = payload;
    }

    public Guid Id { get; }
    public UpdateTrainerPayload Payload { get; }
  }
}
