using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Inventories
{
  public interface IInventoryQuerier
  {
    Task<InventoryModel?> GetAsync(Guid trainerId, Guid itemId, CancellationToken cancellationToken = default);
    Task<ListModel<InventoryModel>> GetPagedAsync(Guid trainerId, ItemCategory? category = null, string? search = null,
      InventorySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
