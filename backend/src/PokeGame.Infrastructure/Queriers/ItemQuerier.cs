using Microsoft.EntityFrameworkCore;
using PokeGame.Core;
using PokeGame.Core.Items;

namespace PokeGame.Infrastructure.Queriers
{
  internal class ItemQuerier : IItemQuerier
  {
    private readonly DbSet<Item> _items;

    public ItemQuerier(PokeGameDbContext dbContext)
    {
      _items = dbContext.Items;
    }

    public async Task<Item?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _items.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Item>> GetPagedAsync(ItemCategory? category, string? search,
      ItemSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Item> query = _items.ApplyTracking(readOnly);

      if (category.HasValue)
      {
        query = query.Where(x => x.Category == category.Value);
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
          }
        }
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          ItemSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          ItemSort.Price => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
          ItemSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The item sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      Item[] items = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Item>(items, total);
    }
  }
}
