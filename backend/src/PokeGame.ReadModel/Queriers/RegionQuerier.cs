using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Regions;
using PokeGame.Application.Regions.Models;
using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class RegionQuerier : IRegionQuerier
  {
    private readonly DbSet<RegionEntity> _regions;
    private readonly IMappingService _mappingService;

    public RegionQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _regions = readContext.Regions;
      _mappingService = mappingService;
    }

    public async Task<RegionModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      RegionEntity? region = await _regions.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return await _mappingService.MapAsync<RegionModel>(region, cancellationToken);
    }

    public async Task<ListModel<RegionModel>> GetPagedAsync(string? search,
      RegionSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<RegionEntity> query = _regions.AsNoTracking();

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
          RegionSort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          RegionSort.UpdatedOn => desc ? query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn) : query.OrderBy(x => x.UpdatedOn ?? x.CreatedOn),
          _ => throw new ArgumentException($"The region sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      RegionEntity[] regions = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = await _mappingService.MapAsync<RegionModel>(regions, cancellationToken),
        Total = total
      };
    }
  }
}
