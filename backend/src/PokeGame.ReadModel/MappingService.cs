using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel
{
  internal class MappingService : IMappingService
  {
    private readonly IMapper _mapper;
    private readonly DbSet<UserEntity> _users;

    public MappingService(IMapper mapper, ReadContext readContext)
    {
      _mapper = mapper;
      _users = readContext.Users;
    }

    public async Task<T?> MapAsync<T>(object? value, CancellationToken cancellationToken)
    {
      var destination = _mapper.Map<T>(value);

      if (value is Entity entity && destination is AggregateModel aggregate)
      {
        IEnumerable<Guid> userIds = new[] { entity.CreatedById, entity.UpdatedById }
          .Where(x => x.HasValue)
          .Select(x => x!.Value)
          .Distinct();

        Dictionary<Guid, ActorModel> users = await _users.AsNoTracking()
          .Where(x => userIds.Contains(x.Id))
          .ProjectTo<ActorModel>(_mapper.ConfigurationProvider)
          .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

        SetActors(aggregate, entity, users);
      }

      return destination;
    }

    public async Task<IEnumerable<T>> MapAsync<T>(IEnumerable<Entity> entities, CancellationToken cancellationToken) where T : AggregateModel
    {
      var aggregates = _mapper.Map<IEnumerable<T>>(entities);

      Dictionary<Guid, Entity> indexedEntities = entities.ToDictionary(x => x.Id, x => x);

      IEnumerable<Guid> userIds = indexedEntities.Values.Select(x => (Guid?)x.CreatedById)
        .Union(entities.Select(x => x.UpdatedById))
        .Where(x => x.HasValue)
        .Select(x => x!.Value)
        .Distinct();

      Dictionary<Guid, ActorModel> users = await _users.AsNoTracking()
        .Where(x => userIds.Contains(x.Id))
        .ProjectTo<ActorModel>(_mapper.ConfigurationProvider)
        .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

      foreach (AggregateModel aggregate in aggregates)
      {
        SetActors(aggregate, indexedEntities[aggregate.Id], users);
      }

      return aggregates;
    }

    private static void SetActors(AggregateModel aggregate, Entity entity, IReadOnlyDictionary<Guid, ActorModel> actors)
    {
      if (actors.TryGetValue(entity.CreatedById, out ActorModel? createdBy))
      {
        aggregate.CreatedBy = createdBy;
      }

      if (entity.UpdatedById.HasValue && actors.TryGetValue(entity.UpdatedById.Value, out ActorModel? updatedBy))
      {
        aggregate.UpdatedBy = updatedBy;
      }
    }
  }
}
