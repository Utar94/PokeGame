using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items
{
  public interface IItemQuerier
  {
    Task<ItemModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListModel<ItemModel>> GetPagedAsync(ItemCategory? category = null, string? search = null,
      ItemSort? sort = null, bool desc = false,
      int? index = null, int? count = null,
      CancellationToken cancellationToken = default);
  }
}
