using PokeGame.Application.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers
{
  public interface ITrainerQuerier
  {
    Task<TrainerModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<TrainerModel>> GetPagedAsync(TrainerGender? gender = null, Guid? regionId = null, string? search = null, Guid? userId = null,
      TrainerSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
