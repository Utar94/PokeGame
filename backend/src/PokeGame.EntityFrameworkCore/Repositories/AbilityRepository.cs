using Logitar;
using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Abilities;
using PokeGame.Domain.Abilities;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class AbilityRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IAbilityRepository
{
  private static readonly string AggregateType = typeof(AbilityAggregate).GetNamespaceQualifiedName();

  private readonly ISqlHelper _sqlHelper;

  public AbilityRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer, ISqlHelper sqlHelper)
    : base(eventBus, eventContext, eventSerializer)
  {
    _sqlHelper = sqlHelper;
  }

  public async Task<IReadOnlyCollection<AbilityAggregate>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<AbilityAggregate>(cancellationToken)).ToArray();
  }

  public async Task<AbilityAggregate?> LoadAsync(AbilityId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<AbilityAggregate?> LoadAsync(AbilityId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<AbilityAggregate>(id.AggregateId, version, cancellationToken);
  }
  public async Task<AbilityAggregate?> LoadAsync(UniqueNameUnit uniqueName, CancellationToken cancellationToken)
  {
    IQuery query = _sqlHelper.QueryFrom(EventDb.Events.Table).SelectAll(EventDb.Events.Table)
      .Join(PokemonDb.Abilities.AggregateId, EventDb.Events.AggregateId,
        new OperatorCondition(EventDb.Events.AggregateType, Operators.IsEqualTo(AggregateType))
        )
      .Where(PokemonDb.Abilities.UniqueNameNormalized, Operators.IsEqualTo(PokemonDb.Normalize(uniqueName.Value)))
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<AbilityAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }

  public async Task SaveAsync(AbilityAggregate ability, CancellationToken cancellationToken)
  {
    await base.SaveAsync(ability, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<AbilityAggregate> abilities, CancellationToken cancellationToken)
  {
    await base.SaveAsync(abilities, cancellationToken);
  }
}
