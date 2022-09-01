namespace PokeGame.Core.Items
{
  public interface IItemQuerier
  {
    Task<Item?> GetAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Item>> GetPagedAsync(Category? category = null, string? search = null,
      ItemSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
