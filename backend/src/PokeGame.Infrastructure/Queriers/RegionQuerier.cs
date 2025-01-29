using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Regions;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain;
using PokeGame.Domain.Regions;
using PokeGame.Infrastructure.Actors;
using PokeGame.Infrastructure.Entities;
using PokeGame.Infrastructure.PokeGameDb;

namespace PokeGame.Infrastructure.Queriers;

internal class RegionQuerier : IRegionQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<RegionEntity> _regions;

  public RegionQuerier(IActorService actorService, PokeGameContext context)
  {
    _actorService = actorService;
    _regions = context.Regions;
  }

  public async Task<RegionId?> FindIdAsync(UniqueName uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = Helper.Normalize(uniqueName);

    string? streamId = await _regions.AsNoTracking()
      .Where(x => x.UniqueNameNormalized == uniqueNameNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);

    return streamId == null ? null : new RegionId(streamId);
  }

  public async Task<RegionModel> ReadAsync(Region region, CancellationToken cancellationToken)
  {
    return await ReadAsync(region.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The region entity 'StreamId={region.Id}' could not be found.");
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
  public async Task<RegionModel?> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = Helper.Normalize(uniqueName);

    RegionEntity? region = await _regions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return region == null ? null : await MapAsync(region, cancellationToken);
  }

  private async Task<RegionModel> MapAsync(RegionEntity region, CancellationToken cancellationToken)
  {
    return (await MapAsync([region], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<RegionModel>> MapAsync(IEnumerable<RegionEntity> regions, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = regions.SelectMany(region => region.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return regions.Select(mapper.ToRegion).ToArray();
  }
}
