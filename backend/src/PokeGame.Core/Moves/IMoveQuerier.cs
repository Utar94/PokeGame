namespace PokeGame.Core.Moves
{
  public interface IMoveQuerier
  {
    Task<Move?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Move>> GetPagedAsync(string? search = null, PokemonType? type = null,
      MoveSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
