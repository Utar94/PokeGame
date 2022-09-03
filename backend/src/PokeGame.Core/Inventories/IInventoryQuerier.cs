using PokeGame.Core.Items;

namespace PokeGame.Core.Inventories
{
  public interface IInventoryQuerier
  {
    Task<Inventory?> GetAsync(int trainerId, int itemId, bool readOnly = false, CancellationToken cancellationToken = default);
    Task<PagedList<Inventory>> GetPagedAsync(int trainerId, ItemCategory? category = null, string? search = null,
      InventorySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      bool readOnly = false, CancellationToken cancellationToken = default);
  }
}
