using PokeGame.Application.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Application.Trainers
{
  public interface ITrainerService
  {
    Task<TrainerModel> CreateAsync(CreateTrainerPayload payload, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TrainerModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<TrainerModel>> GetAsync(TrainerGender? gender = null, Region? region = null, string? search = null, Guid? userId = null,
      TrainerSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<TrainerModel> UpdateAsync(Guid id, UpdateTrainerPayload payload, CancellationToken cancellationToken = default);
  }
}
