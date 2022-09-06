using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Inventories;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class InventoryQuerier : IInventoryQuerier
  {
    private readonly DbSet<Entities.Inventory> _inventory;
    private readonly IMapper _mapper;

    public InventoryQuerier(IMapper mapper, ReadContext readContext)
    {
      _inventory = readContext.Inventory;
      _mapper = mapper;
    }

    public async Task<InventoryModel?> GetAsync(Guid trainerId, Guid itemId, CancellationToken cancellationToken)
    {
      Entities.Inventory? inventory = await _inventory.AsNoTracking()
        .Include(x => x.Item)
        .Include(x => x.Trainer)
        .SingleOrDefaultAsync(x => x.Trainer!.Id == trainerId && x.Item!.Id == itemId, cancellationToken);

      return inventory == null ? null : _mapper.Map<InventoryModel>(inventory);
    }

    public async Task<ListModel<InventoryModel>> GetPagedAsync(Guid trainerId, ItemCategory? category, string? search,
      InventorySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<Entities.Inventory> query = _inventory.AsNoTracking()
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

      Entities.Inventory[] inventory = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<InventoryModel>>(inventory),
        Total = total
      };
    }
  }
}
