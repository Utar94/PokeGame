using Microsoft.EntityFrameworkCore;
using PokeGame.Core;
using PokeGame.Core.Species;

namespace PokeGame.Infrastructure.Queriers
{
  internal class SpeciesQuerier : ISpeciesQuerier
  {
    private readonly DbSet<Species> _species;

    public SpeciesQuerier(PokeGameDbContext dbContext)
    {
      _species = dbContext.Species;
    }

    public async Task<Species?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _species.ApplyTracking(readOnly)
        .Include(x => x.Ability)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Species>> GetPagedAsync(string? search, PokemonType? type,
      SpeciesSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Species> query = _species.ApplyTracking(readOnly)
        .Include(x => x.Ability);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => x.Number.ToString() == term
              || EF.Functions.ILike(x.Name, pattern)
              || (x.Category != null && EF.Functions.ILike(x.Category, pattern)));
          }
        }
      }
      if (type.HasValue)
      {
        query = query.Where(x => x.PrimaryType == type.Value || x.SecondaryType == type.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          SpeciesSort.Category => desc ? query.OrderByDescending(x => x.Category) : query.OrderBy(x => x.Category),
          SpeciesSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          SpeciesSort.Number => desc ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number),
          SpeciesSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The species sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      Species[] species = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Species>(species, total);
    }
  }
}
