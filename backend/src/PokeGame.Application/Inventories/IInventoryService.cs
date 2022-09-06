using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Inventories
{
  public interface IInventoryService
  {
    Task<InventoryModel> AddAsync(Guid trainerId, Guid itemId, ushort quantity, bool buy = false, CancellationToken cancellationToken = default);
    Task<InventoryModel> GetAsync(Guid trainerId, Guid itemId, CancellationToken cancellationToken = default);
    Task<ListModel<InventoryModel>> GetAsync(Guid trainerId, ItemCategory? category = null, string? search = null,
      InventorySort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<InventoryModel> RemoveAsync(Guid trainerId, Guid itemId, ushort quantity, bool sell = false, CancellationToken cancellationToken = default);
  }
}
