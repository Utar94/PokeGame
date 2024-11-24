using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Actors;
using PokeGame.Application.Species;
using PokeGame.Contracts.Species;
using PokeGame.Domain;
using PokeGame.Domain.Species;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Queriers;

internal class PokemonSpeciesQuerier : IPokemonSpeciesQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<PokemonSpeciesEntity> _species;
  private readonly ISqlHelper _sqlHelper;

  public PokemonSpeciesQuerier(IActorService actorService, PokeGameContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _species = context.PokemonSpecies;
    _sqlHelper = sqlHelper;
  }

  public async Task<SpeciesId?> FindIdAsync(UniqueName uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = PokeGameDb.Helper.Normalize(uniqueName.Value);

    string? aggregateId = await _species.AsNoTracking()
      .Where(x => x.UniqueNameNormalized == uniqueNameNormalized)
      .Select(x => x.AggregateId)
      .SingleOrDefaultAsync(cancellationToken);

    return aggregateId == null ? null : new SpeciesId(aggregateId);
  }

  public async Task<SpeciesModel> ReadAsync(PokemonSpecies species, CancellationToken cancellationToken)
  {
    return await ReadAsync(species.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The species entity 'AggregateId={species.Id}' could not be found.");
  }
  public async Task<SpeciesModel?> ReadAsync(SpeciesId id, CancellationToken cancellationToken)
  {
    return await ReadAsync(id.ToGuid(), cancellationToken);
  }
  public async Task<SpeciesModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    PokemonSpeciesEntity? species = await _species.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return species == null ? null : await MapAsync(species, cancellationToken);
  }
  public async Task<SpeciesModel?> ReadAsync(int number, CancellationToken cancellationToken)
  {
    PokemonSpeciesEntity? species = await _species.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Number == number, cancellationToken);

    return species == null ? null : await MapAsync(species, cancellationToken);
  }
  public async Task<SpeciesModel?> ReadAsync(string uniqueName, CancellationToken cancellationToken)
  {
    string uniqueNameNormalized = PokeGameDb.Helper.Normalize(uniqueName);

    PokemonSpeciesEntity? species = await _species.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return species == null ? null : await MapAsync(species, cancellationToken);
  }

  public async Task<SearchResults<SpeciesModel>> SearchAsync(SearchSpeciesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(PokeGameDb.PokemonSpecies.Table).SelectAll(PokeGameDb.PokemonSpecies.Table)
      .ApplyIdFilter(payload, PokeGameDb.PokemonSpecies.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, PokeGameDb.PokemonSpecies.UniqueName, PokeGameDb.PokemonSpecies.DisplayName);

    if (payload.Category.HasValue)
    {
      builder.Where(PokeGameDb.PokemonSpecies.Category, Operators.IsEqualTo(payload.Category.Value.ToString()));
    }
    if (payload.LevelingRate.HasValue)
    {
      builder.Where(PokeGameDb.PokemonSpecies.LevelingRate, Operators.IsEqualTo(payload.LevelingRate.Value.ToString()));
    }
    // TODO(fpion): RegionId

    IQueryable<PokemonSpeciesEntity> query = _species.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<PokemonSpeciesEntity>? ordered = null;
    foreach (SpeciesSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case SpeciesSort.BaseHappiness:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.BaseHappiness) : query.OrderBy(x => x.BaseHappiness))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.BaseHappiness) : ordered.ThenBy(x => x.BaseHappiness));
          break;
        case SpeciesSort.CaptureRate:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CaptureRate) : query.OrderBy(x => x.CaptureRate))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CaptureRate) : ordered.ThenBy(x => x.CaptureRate));
          break;
        case SpeciesSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case SpeciesSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case SpeciesSort.Number:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Number) : ordered.ThenBy(x => x.Number));
          break;
        case SpeciesSort.UniqueName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueName) : query.OrderBy(x => x.UniqueName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueName) : ordered.ThenBy(x => x.UniqueName));
          break;
        case SpeciesSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    PokemonSpeciesEntity[] species = await query.ToArrayAsync(cancellationToken);
    IEnumerable<SpeciesModel> items = await MapAsync(species, cancellationToken);

    return new SearchResults<SpeciesModel>(items, total);
  }

  private async Task<SpeciesModel> MapAsync(PokemonSpeciesEntity species, CancellationToken cancellationToken)
    => (await MapAsync([species], cancellationToken)).Single();
  private async Task<IReadOnlyCollection<SpeciesModel>> MapAsync(IEnumerable<PokemonSpeciesEntity> species, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = species.SelectMany(species => species.GetActorIds());
    IReadOnlyCollection<Actor> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return species.Select(mapper.ToPokemonSpecies).ToArray().AsReadOnly();
  }
}
