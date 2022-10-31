using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Queries
{
  public class GetTrainersQuery : IRequest<ListModel<TrainerModel>>
  {
    public TrainerGender? Gender { get; set; }
    public Guid? RegionId { get; set; }
    public string? Search { get; set; }
    public Guid? UserId { get; set; }

    public TrainerSort? Sort { get; set; }
    public bool Desc { get; set; }

    public int? Index { get; set; }
    public int? Count { get; set; }
  }
}
