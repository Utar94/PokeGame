namespace PokeGame.Core.Abilities
{
  public interface IAbilityQuerier
  {
    Task<Ability?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Ability>> GetPagedAsync(string? search = null,
      AbilitySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
