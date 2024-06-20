using Logitar;
using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Items;
using PokeGame.Domain.Items;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class ItemRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IItemRepository
{
  private static readonly string AggregateType = typeof(ItemAggregate).GetNamespaceQualifiedName();

  private readonly ISqlHelper _sqlHelper;

  public ItemRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    _sqlHelper = sqlHelper;
  }

  public async Task<IReadOnlyCollection<ItemAggregate>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<ItemAggregate>(cancellationToken)).ToArray();
  }

  public async Task<ItemAggregate?> LoadAsync(ItemId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<ItemAggregate?> LoadAsync(ItemId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<ItemAggregate>(id.AggregateId, version, cancellationToken);
  }

  public async Task<ItemAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken)
  {
    IQuery query = _sqlHelper.QueryFrom(EventDb.Events.Table).SelectAll(EventDb.Events.Table)
      .Join(PokemonDb.Items.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(PokemonDb.Items.UniqueNameNormalized, Operators.IsEqualTo(PokemonDb.Normalize(uniqueName.Value)))
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<ItemAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public async Task SaveAsync(ItemAggregate item, CancellationToken cancellationToken)
  {
    await base.SaveAsync(item, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<ItemAggregate> items, CancellationToken cancellationToken)
  {
    await base.SaveAsync(items, cancellationToken);
  }
}
