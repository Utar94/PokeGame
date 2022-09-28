using MediatR;

namespace PokeGame.Application.Trainers.Mutations
{
  public class DeleteTrainerMutation : IRequest
  {
    public DeleteTrainerMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
