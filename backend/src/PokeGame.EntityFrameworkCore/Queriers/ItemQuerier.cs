using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Items;
using PokeGame.Contracts.Items;
using PokeGame.Domain.Items;
using PokeGame.EntityFrameworkCore.Actors;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Queriers;

internal class ItemQuerier : IItemQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<ItemEntity> _items;
  private readonly ISqlHelper _sqlHelper;

  public ItemQuerier(IActorService actorService, PokemonContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _items = context.Items;
    _sqlHelper = sqlHelper;
  }

  public async Task<Item> ReadAsync(ItemAggregate item, CancellationToken cancellationToken)
  {
    return await ReadAsync(item.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The item entity 'AggregateId={item.Id.AggregateId}' could not be found.");
  }
  public async Task<Item?> ReadAsync(ItemId id, CancellationToken cancellationToken)
  {
    return await ReadAsync(id.ToGuid(), cancellationToken);
  }
  public async Task<Item?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    string aggregateId = new AggregateId(id).Value;

    ItemEntity? item = await _items.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId, cancellationToken);

    return item == null ? null : await MapAsync(item, cancellationToken);
  }

  public async Task<Item?> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = PokemonDb.Normalize(uniqueName);

    ItemEntity? item = await _items.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return item == null ? null : await MapAsync(item, cancellationToken);
  }

  public async Task<SearchResults<Item>> SearchAsync(SearchItemsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(PokemonDb.Items.Table).SelectAll(PokemonDb.Items.Table)
      .ApplyIdInFilter(PokemonDb.Items.AggregateId, payload);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, PokemonDb.Items.UniqueName, PokemonDb.Items.DisplayName);

    if (payload.Category.HasValue)
    {
      builder.Where(PokemonDb.Items.Category, Operators.IsEqualTo((int)payload.Category.Value));
    }

    IQueryable<ItemEntity> query = _items.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<ItemEntity>? ordered = null;
    foreach (ItemSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case ItemSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case ItemSort.Price:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Price) : ordered.ThenBy(x => x.Price));
          break;
        case ItemSort.UniqueName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueName) : query.OrderBy(x => x.UniqueName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueName) : ordered.ThenBy(x => x.UniqueName));
          break;
        case ItemSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    ItemEntity[] entities = await query.ToArrayAsync(cancellationToken);
    IEnumerable<Item> items = await MapAsync(entities, cancellationToken);

    return new SearchResults<Item>(items, total);
  }

  private async Task<Item> MapAsync(ItemEntity item, CancellationToken cancellationToken)
  {
    return (await MapAsync([item], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<Item>> MapAsync(IEnumerable<ItemEntity> items, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = items.SelectMany(item => item.GetActorIds());
    IReadOnlyCollection<Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return items.Select(mapper.ToItem).ToArray();
  }
}
