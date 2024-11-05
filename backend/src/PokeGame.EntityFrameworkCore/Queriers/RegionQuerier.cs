using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Actors;
using PokeGame.Application.Regions;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Regions;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Queriers;

internal class RegionQuerier : IRegionQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<RegionEntity> _regions;
  private readonly ISqlHelper _sqlHelper;

  public RegionQuerier(IActorService actorService, PokeGameContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _regions = context.Regions;
    _sqlHelper = sqlHelper;
  }

  public async Task<RegionModel> ReadAsync(Region region, CancellationToken cancellationToken)
  {
    return await ReadAsync(region.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The region entity 'AggregateId={region.Id}' could not be found.");
  }
  public async Task<RegionModel?> ReadAsync(RegionId id, CancellationToken cancellationToken)
  {
    return await ReadAsync(id.ToGuid(), cancellationToken);
  }
  public async Task<RegionModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    RegionEntity? region = await _regions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return region == null ? null : await MapAsync(region, cancellationToken);
  }

  public async Task<SearchResults<RegionModel>> SearchAsync(SearchRegionsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(PokeGameDb.Regions.Table).SelectAll(PokeGameDb.Regions.Table)
      .ApplyIdFilter(payload, PokeGameDb.Regions.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, PokeGameDb.Regions.Name);

    IQueryable<RegionEntity> query = _regions.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<RegionEntity>? ordered = null;
    foreach (RegionSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case RegionSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case RegionSort.Name:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Name) : ordered.ThenBy(x => x.Name));
          break;
        case RegionSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    RegionEntity[] regions = await query.ToArrayAsync(cancellationToken);
    IEnumerable<RegionModel> items = await MapAsync(regions, cancellationToken);

    return new SearchResults<RegionModel>(items, total);
  }

  private async Task<RegionModel> MapAsync(RegionEntity region, CancellationToken cancellationToken)
    => (await MapAsync([region], cancellationToken)).Single();
  private async Task<IReadOnlyCollection<RegionModel>> MapAsync(IEnumerable<RegionEntity> regions, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = regions.SelectMany(region => region.GetActorIds());
    IReadOnlyCollection<Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return regions.Select(mapper.ToRegion).ToArray().AsReadOnly();
  }
}
