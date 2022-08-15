using Microsoft.EntityFrameworkCore;
using PokeGame.Core;
using PokeGame.Core.Abilities;

namespace PokeGame.Infrastructure.Queriers
{
  internal class AbilityQuerier : IAbilityQuerier
  {
    private readonly DbSet<Ability> _abilities;

    public AbilityQuerier(PokeGameDbContext dbContext)
    {
      _abilities = dbContext.Abilities;
    }

    public async Task<Ability?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _abilities.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Ability>> GetPagedAsync(string? search,
      AbilitySort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Ability> query = _abilities.ApplyTracking(readOnly);

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
          AbilitySort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          AbilitySort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The ability sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      Ability[] abilities = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Ability>(abilities, total);
    }
  }
}
