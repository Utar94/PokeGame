using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Moves;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;
using PokeGame.EntityFrameworkCore.Actors;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Queriers;

internal class MoveQuerier : IMoveQuerier
{
  private readonly DbSet<MoveEntity> _moves;
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;

  public MoveQuerier(IActorService actorService, PokemonContext context, ISqlHelper sqlHelper)
  {
    _moves = context.Moves;
    _actorService = actorService;
    _sqlHelper = sqlHelper;
  }

  public async Task<Move> ReadAsync(MoveAggregate move, CancellationToken cancellationToken)
  {
    return await ReadAsync(move.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The move entity 'AggregateId={move.Id.AggregateId}' could not be found.");
  }
  public async Task<Move?> ReadAsync(MoveId id, CancellationToken cancellationToken)
  {
    return await ReadAsync(id.ToGuid(), cancellationToken);
  }
  public async Task<Move?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    string aggregateId = new AggregateId(id).Value;

    MoveEntity? move = await _moves.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId, cancellationToken);

    return move == null ? null : await MapAsync(move, cancellationToken);
  }

  public async Task<Move?> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = PokemonDb.Normalize(uniqueName);

    MoveEntity? move = await _moves.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return move == null ? null : await MapAsync(move, cancellationToken);
  }

  public async Task<SearchResults<Move>> SearchAsync(SearchMovesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(PokemonDb.Moves.Table).SelectAll(PokemonDb.Moves.Table)
      .ApplyIdInFilter(PokemonDb.Moves.AggregateId, payload);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, PokemonDb.Moves.UniqueName, PokemonDb.Moves.DisplayName);

    if (payload.Type.HasValue)
    {
      builder.Where(PokemonDb.Moves.Type, Operators.IsEqualTo((int)payload.Type.Value));
    }
    if (payload.Category.HasValue)
    {
      builder.Where(PokemonDb.Moves.Category, Operators.IsEqualTo((int)payload.Category.Value));
    }

    IQueryable<MoveEntity> query = _moves.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<MoveEntity>? ordered = null;
    foreach (MoveSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case MoveSort.Accuracy:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Accuracy) : query.OrderBy(x => x.Accuracy))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Accuracy) : ordered.ThenBy(x => x.Accuracy));
          break;
        case MoveSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case MoveSort.Power:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Power) : query.OrderBy(x => x.Power))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Power) : ordered.ThenBy(x => x.Power));
          break;
        case MoveSort.PowerPoints:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.PowerPoints) : query.OrderBy(x => x.PowerPoints))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.PowerPoints) : ordered.ThenBy(x => x.PowerPoints));
          break;
        case MoveSort.UniqueName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueName) : query.OrderBy(x => x.UniqueName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueName) : ordered.ThenBy(x => x.UniqueName));
          break;
        case MoveSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    MoveEntity[] moves = await query.ToArrayAsync(cancellationToken);
    IEnumerable<Move> items = await MapAsync(moves, cancellationToken);

    return new SearchResults<Move>(items, total);
  }

  private async Task<Move> MapAsync(MoveEntity move, CancellationToken cancellationToken)
  {
    return (await MapAsync([move], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<Move>> MapAsync(IEnumerable<MoveEntity> moves, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = moves.SelectMany(move => move.GetActorIds());
    IReadOnlyCollection<Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return moves.Select(mapper.ToMove).ToArray();
  }
}
