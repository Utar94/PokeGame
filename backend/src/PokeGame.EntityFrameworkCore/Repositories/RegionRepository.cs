using Logitar;
using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Regions;
using PokeGame.Domain.Regions;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class RegionRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IRegionRepository
{
  private static readonly string AggregateType = typeof(RegionAggregate).GetNamespaceQualifiedName();

  private readonly ISqlHelper _sqlHelper;

  public RegionRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    _sqlHelper = sqlHelper;
  }

  public async Task<IReadOnlyCollection<RegionAggregate>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<RegionAggregate>(cancellationToken)).ToArray();
  }

  public async Task<RegionAggregate?> LoadAsync(RegionId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<RegionAggregate?> LoadAsync(RegionId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<RegionAggregate>(id.AggregateId, version, cancellationToken);
  }

  public async Task<RegionAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken)
  {
    IQuery query = _sqlHelper.QueryFrom(EventDb.Events.Table).SelectAll(EventDb.Events.Table)
      .Join(PokemonDb.Regions.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(PokemonDb.Regions.UniqueNameNormalized, Operators.IsEqualTo(PokemonDb.Normalize(uniqueName.Value)))
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<RegionAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public async Task SaveAsync(RegionAggregate region, CancellationToken cancellationToken)
  {
    await base.SaveAsync(region, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<RegionAggregate> regions, CancellationToken cancellationToken)
  {
    await base.SaveAsync(regions, cancellationToken);
  }
}
