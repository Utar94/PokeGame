using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Items.Payloads;

namespace PokeGame.Application.Items
{
  public interface IItemService
  {
    Task<ItemModel> CreateAsync(CreateItemPayload payload, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ItemModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<ItemModel>> GetAsync(ItemCategory? category = null, string? search = null,
      ItemSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
    Task<ItemModel> UpdateAsync(Guid id, UpdateItemPayload payload, CancellationToken cancellationToken = default);
  }
}
