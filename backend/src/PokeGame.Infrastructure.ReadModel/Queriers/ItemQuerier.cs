using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Items;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Domain.Items;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class ItemQuerier : IItemQuerier
  {
    private readonly DbSet<ItemEntity> _items;
    private readonly IMapper _mapper;

    public ItemQuerier(IMapper mapper, ReadContext readContext)
    {
      _items = readContext.Items;
      _mapper = mapper;
    }

    public async Task<ItemModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      ItemEntity? item = await _items.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return item == null ? null : _mapper.Map<ItemModel>(item);
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
          ItemSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The item sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      ItemEntity[] items = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<ItemModel>>(items),
        Total = total
      };
    }
  }
}
