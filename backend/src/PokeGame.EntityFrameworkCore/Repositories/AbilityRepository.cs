using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using PokeGame.Domain.Abilities;

namespace PokeGame.EntityFrameworkCore.Repositories;

internal class AbilityRepository : Logitar.EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IAbilityRepository
{
  public AbilityRepository(IEventBus eventBus, EventContext eventContext, IEventSerializer eventSerializer)
    : base(eventBus, eventContext, eventSerializer)
  {
  }

  public async Task<Ability?> LoadAsync(AbilityId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, cancellationToken);
  }
  public async Task<Ability?> LoadAsync(AbilityId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync<Ability>(id.AggregateId, version, cancellationToken);
  }

  public async Task<IReadOnlyCollection<Ability>> LoadAsync(CancellationToken cancellationToken)
  {
    return (await LoadAsync<Ability>(cancellationToken)).ToArray().AsReadOnly();
  }
  public async Task<IReadOnlyCollection<Ability>> LoadAsync(IEnumerable<AbilityId> ids, CancellationToken cancellationToken)
  {
    IEnumerable<AggregateId> aggregateIds = ids.Select(id => id.AggregateId);
    return (await LoadAsync<Ability>(aggregateIds, cancellationToken)).ToArray().AsReadOnly();
  }

  public async Task SaveAsync(Ability ability, CancellationToken cancellationToken)
  {
    await base.SaveAsync(ability, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<Ability> abilities, CancellationToken cancellationToken)
  {
    await base.SaveAsync(abilities, cancellationToken);
  }
}
