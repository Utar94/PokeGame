using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Items;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class ItemQuerier : IItemQuerier
  {
    private readonly DbSet<ItemEntity> _items;
    private readonly IMappingService _mappingService;

    public ItemQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _items = readContext.Items;
      _mappingService = mappingService;
    }

    public async Task<ItemModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      ItemEntity? item = await _items.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return await _mappingService.MapAsync<ItemModel>(item, cancellationToken);
    }

    public async Task<ListModel<ItemModel>> GetPagedAsync(ItemCategory? category, string? search,
      ItemSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<ItemEntity> query = _items.AsNoTracking();

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
          ItemSort.UpdatedOn => desc ? query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn) : query.OrderBy(x => x.UpdatedOn ?? x.CreatedOn),
          _ => throw new ArgumentException($"The item sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      ItemEntity[] items = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = await _mappingService.MapAsync<ItemModel>(items, cancellationToken),
        Total = total
      };
    }
  }
}
