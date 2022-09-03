using Microsoft.EntityFrameworkCore;
using PokeGame.Core;
using PokeGame.Core.Trainers;

namespace PokeGame.Infrastructure.Queriers
{
  internal class TrainerQuerier : ITrainerQuerier
  {
    private readonly DbSet<Trainer> _trainers;

    public TrainerQuerier(PokeGameDbContext dbContext)
    {
      _trainers = dbContext.Trainers;
    }

    public async Task<Trainer?> GetAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _trainers.ApplyTracking(readOnly)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Trainer?> GetWithInventoryAsync(Guid id, bool readOnly, CancellationToken cancellationToken)
    {
      return await _trainers.ApplyTracking(readOnly)
        .Include(x => x.Inventory).ThenInclude(x => x.Item)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedList<Trainer>> GetPagedAsync(TrainerGender? gender, Region? region, string? search, Guid? userId,
      TrainerSort? sort, bool desc,
      int? index, int? count,
      bool readOnly, CancellationToken cancellationToken)
    {
      IQueryable<Trainer> query = _trainers.ApplyTracking(readOnly);

      if (gender.HasValue)
      {
        query = query.Where(x => x.Gender == gender.Value);
      }
      if (region.HasValue)
      {
        query = query.Where(x => x.Region == region.Value);
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.Name, pattern)
              || EF.Functions.ILike(x.Number.ToString(), pattern));
          }
        }
      }
      if (userId.HasValue)
      {
        query = query.Where(x => x.UserId == userId.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          TrainerSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          TrainerSort.Number => desc ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number),
          TrainerSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The trainer sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      Trainer[] trainers = await query.ToArrayAsync(cancellationToken);

      return new PagedList<Trainer>(trainers, total);
    }
  }
}
