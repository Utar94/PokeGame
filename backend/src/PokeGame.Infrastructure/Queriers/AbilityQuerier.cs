using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Abilities;
using PokeGame.Application.Abilities.Models;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Infrastructure.Actors;
using PokeGame.Infrastructure.Entities;
using PokeGame.Infrastructure.PokeGameDb;

namespace PokeGame.Infrastructure.Queriers;

internal class AbilityQuerier : IAbilityQuerier
{
  private readonly DbSet<AbilityEntity> _abilities;
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;

  public AbilityQuerier(IActorService actorService, PokeGameContext context, ISqlHelper sqlHelper)
  {
    _abilities = context.Abilities;
    _actorService = actorService;
    _sqlHelper = sqlHelper;
  }

  public async Task<AbilityId?> FindIdAsync(UniqueName uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = Helper.Normalize(uniqueName);

    string? streamId = await _abilities.AsNoTracking()
      .Where(x => x.UniqueNameNormalized == uniqueNameNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);

    return streamId == null ? null : new AbilityId(streamId);
  }

  public async Task<AbilityModel> ReadAsync(Ability ability, CancellationToken cancellationToken)
  {
    return await ReadAsync(ability.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The ability entity 'StreamId={ability.Id}' could not be found.");
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
  public async Task<AbilityModel?> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = Helper.Normalize(uniqueName);

    AbilityEntity? ability = await _abilities.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return ability == null ? null : await MapAsync(ability, cancellationToken);
  }

  public async Task<SearchResults<AbilityModel>> SearchAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(Abilities.Table).SelectAll(Abilities.Table)
      .ApplyIdFilter(payload.Ids, Abilities.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, Abilities.UniqueName, Abilities.DisplayName);

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
    IReadOnlyCollection<AbilityModel> items = await MapAsync(abilities, cancellationToken);

    return new SearchResults<AbilityModel>(items, total);
  }

  private async Task<AbilityModel> MapAsync(AbilityEntity ability, CancellationToken cancellationToken)
  {
    return (await MapAsync([ability], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<AbilityModel>> MapAsync(IEnumerable<AbilityEntity> abilities, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = abilities.SelectMany(ability => ability.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return abilities.Select(mapper.ToAbility).ToArray();
  }
}
