using MediatR;
using PokeGame.Application.Trainers.Models;

namespace PokeGame.Application.Trainers.Queries
{
  public class GetTrainerQuery : IRequest<TrainerModel?>
  {
    public GetTrainerQuery(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
