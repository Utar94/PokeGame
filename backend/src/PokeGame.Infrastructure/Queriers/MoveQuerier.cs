using Microsoft.EntityFrameworkCore;
using PokeGame.Core;
using PokeGame.Core.Moves;

namespace PokeGame.Infrastructure.Queriers
{
  internal class MoveQuerier : IMoveQuerier
  {
    private readonly DbSet<Move> _moves;

    public MoveQuerier(PokeGameDbContext dbContext)
    {
      _moves = dbContext.Moves;
    }

    public async Task<Move?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _moves.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Move>> GetPagedAsync(string? search, PokemonType? type,
      MoveSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Move> query = _moves.ApplyTracking(readOnly);

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
      if (type.HasValue)
      {
        query = query.Where(x => x.Type == type.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          MoveSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          MoveSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The move sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      Move[] moves = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Move>(moves, total);
    }
  }
}
