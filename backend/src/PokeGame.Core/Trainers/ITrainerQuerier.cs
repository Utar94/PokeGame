namespace PokeGame.Core.Trainers
{
  public interface ITrainerQuerier
  {
    Task<Trainer?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<Trainer?> GetWithInventoryAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Trainer>> GetPagedAsync(TrainerGender? gender = null, Region? region = null, string? search = null, Guid? userId = null,
      TrainerSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
