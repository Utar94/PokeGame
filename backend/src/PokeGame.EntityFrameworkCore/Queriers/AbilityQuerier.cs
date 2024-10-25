using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Abilities;
using PokeGame.Application.Actors;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Queriers;

internal class AbilityQuerier : IAbilityQuerier
{
  private readonly DbSet<AbilityEntity> _abilities;
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;

  public AbilityQuerier(IActorService actorService, PokeGameContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _abilities = context.Abilities;
    _sqlHelper = sqlHelper;
  }

  public async Task<AbilityModel> ReadAsync(Ability ability, CancellationToken cancellationToken)
  {
    return await ReadAsync(ability.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The ability entity 'AggregateId={ability.Id}' could not be found.");
  }
  public async Task<AbilityModel?> ReadAsync(AbilityId id, CancellationToken cancellationToken)
  {
    return await ReadAsync(id.ToGuid(), cancellationToken);
  }
  public async Task<AbilityModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    AbilityEntity? ability = await _abilities.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return ability == null ? null : await MapAsync(ability, cancellationToken);
  }

  public async Task<SearchResults<AbilityModel>> SearchAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(PokeGameDb.Abilities.Table).SelectAll(PokeGameDb.Abilities.Table)
      .ApplyIdFilter(payload, PokeGameDb.Abilities.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, PokeGameDb.Abilities.Name);

    if (payload.Kind.HasValue)
    {
      builder.Where(PokeGameDb.Abilities.Kind, Operators.IsEqualTo(payload.Kind.Value.ToString()));
    }

    IQueryable<AbilityEntity> query = _abilities.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<AbilityEntity>? ordered = null;
    foreach (AbilitySortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case AbilitySort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case AbilitySort.Name:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Name) : ordered.ThenBy(x => x.Name));
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
    IEnumerable<AbilityModel> items = await MapAsync(abilities, cancellationToken);

    return new SearchResults<AbilityModel>(items, total);
  }

  private async Task<AbilityModel> MapAsync(AbilityEntity ability, CancellationToken cancellationToken)
    => (await MapAsync([ability], cancellationToken)).Single();
  private async Task<IReadOnlyCollection<AbilityModel>> MapAsync(IEnumerable<AbilityEntity> abilities, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = abilities.SelectMany(ability => ability.GetActorIds());
    IReadOnlyCollection<Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return abilities.Select(mapper.ToAbility).ToArray().AsReadOnly();
  }
}
