using Microsoft.EntityFrameworkCore;
using PokeGame.Core;
using PokeGame.Core.Inventories;
using PokeGame.Core.Items;

namespace PokeGame.Infrastructure.Queriers
{
  internal class InventoryQuerier : IInventoryQuerier
  {
    private readonly DbSet<Inventory> _inventory;

    public InventoryQuerier(PokeGameDbContext dbContext)
    {
      _inventory = dbContext.Inventory;
    }

    public async Task<Inventory?> GetAsync(int trainerId, int itemId, bool readOnly, CancellationToken cancellationToken)
    {
      return await _inventory.ApplyTracking(readOnly)
        .Include(x => x.Item)
        .SingleOrDefaultAsync(x => x.TrainerId == trainerId && x.ItemId == itemId, cancellationToken);
    }

    public async Task<PagedList<Inventory>> GetPagedAsync(int trainerId, ItemCategory? category, string? search,
      InventorySort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Inventory> query = _inventory.ApplyTracking(readOnly)
        .Include(x => x.Item)
        .Where(x => x.TrainerId == trainerId);

      if (category.HasValue)
      {
        query = query.Where(x => x.Item!.Category == category.Value);
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.Item!.Name, pattern));
          }
        }
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          InventorySort.ItemName => desc ? query.OrderByDescending(x => x.Item!.Name) : query.OrderBy(x => x.Item!.Name),
          InventorySort.ItemPrice => desc ? query.OrderByDescending(x => x.Item!.Price) : query.OrderBy(x => x.Item!.Price),
          InventorySort.Quantity => desc ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity),
          _ => throw new ArgumentException($"The inventory sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      Inventory[] inventory = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Inventory>(inventory, total);
    }
  }
}
