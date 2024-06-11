using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Abilities;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;
using PokeGame.EntityFrameworkCore.Actors;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Queriers;

internal class AbilityQuerier : IAbilityQuerier
{
  private readonly DbSet<AbilityEntity> _abilities;
  private readonly IActorService _actorService;

  public AbilityQuerier(IActorService actorService, PokemonContext context)
  {
    _abilities = context.Abilities;
    _actorService = actorService;
  }

  public async Task<Ability> ReadAsync(AbilityAggregate ability, CancellationToken cancellationToken)
  {
    return await ReadAsync(ability.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The ability entity 'AggregateId={ability.Id.AggregateId}' could not be found.");
  }
  public async Task<Ability?> ReadAsync(AbilityId id, CancellationToken cancellationToken)
  {
    return await ReadAsync(id.ToGuid(), cancellationToken);
  }
  public async Task<Ability?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    string aggregateId = new AggregateId(id).Value;

    AbilityEntity? ability = await _abilities.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId, cancellationToken);

    return ability == null ? null : await MapAsync(ability, cancellationToken);
  }

  public async Task<Ability?> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = PokemonDb.Normalize(uniqueName);

    AbilityEntity? ability = await _abilities.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return ability == null ? null : await MapAsync(ability, cancellationToken);
  }

  private async Task<Ability> MapAsync(AbilityEntity ability, CancellationToken cancellationToken)
  {
    return (await MapAsync([ability], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<Ability>> MapAsync(IEnumerable<AbilityEntity> abilities, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = abilities.SelectMany(ability => ability.GetActorIds());
    IReadOnlyCollection<Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return abilities.Select(mapper.ToAbility).ToArray();
  }
}
