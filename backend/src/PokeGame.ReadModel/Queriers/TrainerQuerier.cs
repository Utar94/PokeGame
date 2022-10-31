using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class TrainerQuerier : ITrainerQuerier
  {
    private readonly IMappingService _mappingService;
    private readonly DbSet<TrainerEntity> _trainers;

    public TrainerQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _mappingService = mappingService;
      _trainers = readContext.Trainers;
    }

    public async Task<TrainerModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      TrainerEntity? trainer = await _trainers.AsNoTracking()
        .Include(x => x.Region)
        .Include(x => x.User)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return await _mappingService.MapAsync<TrainerModel>(trainer, cancellationToken);
    }

    public async Task<ListModel<TrainerModel>> GetPagedAsync(TrainerGender? gender, Region? region, string? search, Guid? userId,
      TrainerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<TrainerEntity> query = _trainers.AsNoTracking()
        .Include(x => x.Region)
        .Include(x => x.User);

      if (gender.HasValue)
      {
        query = query.Where(x => x.Gender == gender.Value);
      }
      if (region.HasValue)
      {
        query = query.Where(x => x.LegacyRegion == region.Value);
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
        query = query.Where(x => x.User!.Id == userId.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          TrainerSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          TrainerSort.Number => desc ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number),
          TrainerSort.UpdatedOn => desc ? query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn) : query.OrderBy(x => x.UpdatedOn ?? x.CreatedOn),
          _ => throw new ArgumentException($"The trainer sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      TrainerEntity[] trainers = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = await _mappingService.MapAsync<TrainerModel>(trainers, cancellationToken),
        Total = total
      };
    }
  }
}
