using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class TrainerQuerier : ITrainerQuerier
  {
    private readonly IMapper _mapper;
    private readonly DbSet<TrainerEntity> _trainers;

    public TrainerQuerier(IMapper mapper, ReadContext readContext)
    {
      _mapper = mapper;
      _trainers = readContext.Trainers;
    }

    public async Task<TrainerModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      TrainerEntity? trainer = await _trainers.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return trainer == null ? null : _mapper.Map<TrainerModel>(trainer);
    }

    public async Task<ListModel<TrainerModel>> GetPagedAsync(TrainerGender? gender, Region? region, string? search, Guid? userId,
      TrainerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<TrainerEntity> query = _trainers.AsNoTracking();

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

      TrainerEntity[] trainers = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<TrainerModel>>(trainers),
        Total = total
      };
    }
  }
}
