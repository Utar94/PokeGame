using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Inventories;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class InventoryQuerier : IInventoryQuerier
  {
    private readonly DbSet<InventoryEntity> _inventory;
    private readonly IMappingService _mappingService;

    public InventoryQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _inventory = readContext.Inventory;
      _mappingService = mappingService;
    }

    public async Task<InventoryModel?> GetAsync(Guid trainerId, Guid itemId, CancellationToken cancellationToken)
    {
      InventoryEntity? inventory = await _inventory.AsNoTracking()
        .Include(x => x.Item)
        .Include(x => x.Trainer)
        .SingleOrDefaultAsync(x => x.Trainer!.Id == trainerId && x.Item!.Id == itemId, cancellationToken);

      return await _mappingService.MapAsync<InventoryModel>(inventory, cancellationToken);
    }

    public async Task<ListModel<InventoryModel>> GetPagedAsync(Guid trainerId, ItemCategory? category, string? search,
      InventorySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<InventoryEntity> query = _inventory.AsNoTracking()
        .Include(x => x.Item)
        .Include(x => x.Trainer)
        .Where(x => x.Trainer!.Id == trainerId);

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

      InventoryEntity[] inventory = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = (await _mappingService.MapAsync<IEnumerable<InventoryModel>>(inventory, cancellationToken))!,
        Total = total
      };
    }
  }
}
