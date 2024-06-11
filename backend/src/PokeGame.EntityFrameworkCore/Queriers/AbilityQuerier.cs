using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
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
  private readonly ISqlHelper _sqlHelper;

  public AbilityQuerier(IActorService actorService, PokemonContext context, ISqlHelper sqlHelper)
  {
    _abilities = context.Abilities;
    _actorService = actorService;
    _sqlHelper = sqlHelper;
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

  public async Task<SearchResults<Ability>> SearchAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(PokemonDb.Abilities.Table).SelectAll(PokemonDb.Abilities.Table)
      .ApplyIdInFilter(PokemonDb.Abilities.AggregateId, payload);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, PokemonDb.Abilities.UniqueName, PokemonDb.Abilities.DisplayName);

    IQueryable<AbilityEntity> query = _abilities.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<AbilityEntity>? ordered = null;
    foreach (AbilitySortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case AbilitySort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case AbilitySort.UniqueName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueName) : query.OrderBy(x => x.UniqueName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueName) : ordered.ThenBy(x => x.UniqueName));
          break;
        case AbilitySort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    AbilityEntity[] abilities = await query.ToArrayAsync(cancellationToken);
    IEnumerable<Ability> items = await MapAsync(abilities, cancellationToken);

    return new SearchResults<Ability>(items, total);
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
