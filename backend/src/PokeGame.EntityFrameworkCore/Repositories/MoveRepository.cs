using Logitar;
using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Moves;
using PokeGame.Domain.Moves;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class MoveRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IMoveRepository
{
  private static readonly string AggregateType = typeof(MoveAggregate).GetNamespaceQualifiedName();

  private readonly ISqlHelper _sqlHelper;

  public MoveRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    _sqlHelper = sqlHelper;
  }

  public async Task<IReadOnlyCollection<MoveAggregate>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<MoveAggregate>(cancellationToken)).ToArray();
  }

  public async Task<MoveAggregate?> LoadAsync(MoveId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<MoveAggregate?> LoadAsync(MoveId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<MoveAggregate>(id.AggregateId, version, cancellationToken);
  }

  public async Task<MoveAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken)
  {
    IQuery query = _sqlHelper.QueryFrom(EventDb.Events.Table).SelectAll(EventDb.Events.Table)
      .Join(PokemonDb.Moves.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(PokemonDb.Moves.UniqueNameNormalized, Operators.IsEqualTo(PokemonDb.Normalize(uniqueName.Value)))
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<MoveAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public async Task SaveAsync(MoveAggregate move, CancellationToken cancellationToken)
  {
    await base.SaveAsync(move, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<MoveAggregate> moves, CancellationToken cancellationToken)
  {
    await base.SaveAsync(moves, cancellationToken);
  }
}
