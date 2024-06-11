using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Caching;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Actors;

internal class ActorService : IActorService
{
  private readonly DbSet<ActorEntity> _actors;
  private readonly ICacheService _cacheService;

  public ActorService(ICacheService cacheService, PokemonContext context)
  {
    _actors = context.Actors;
    _cacheService = cacheService;
  }

  public virtual async Task<IReadOnlyCollection<Actor>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken)
  {
    int capacity = ids.Count();

    Dictionary<ActorId, Actor> actors = new(capacity);

    HashSet<string> missingIds = new(capacity);
    foreach (ActorId id in ids)
    {
      Actor? actor = _cacheService.GetActor(id);
      if (actor == null)
      {
        missingIds.Add(id.Value);
      }
      else
      {
        actors[id] = actor;
        _cacheService.SetActor(actor);
      }
    }

    if (missingIds.Count > 0)
    {
      ActorEntity[] entities = await _actors.AsNoTracking()
        .Where(a => missingIds.Contains(a.Id))
        .ToArrayAsync(cancellationToken);

      foreach (ActorEntity entity in entities)
      {
        ActorId id = new(entity.Id);
        Actor actor = Mapper.ToActor(entity);

        actors[id] = actor;
        _cacheService.SetActor(actor);
      }
    }

    return actors.Values;
  }
}
