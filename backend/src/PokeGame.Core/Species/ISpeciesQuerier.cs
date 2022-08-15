namespace PokeGame.Core.Species
{
  public interface ISpeciesQuerier
  {
    Task<Species?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Species>> GetPagedAsync(string? search = null, PokemonType? type = null,
      SpeciesSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
