using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Microsoft.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.Entities;
using PokeGame.Infrastructure.Caching;

namespace PokeGame.EntityFrameworkCore.Actors;

internal class ActorService : IActorService
{
  private readonly ICacheService _cacheService;
  private readonly PokeGameContext _context;

  public ActorService(ICacheService cacheService, PokeGameContext context)
  {
    _cacheService = cacheService;
    _context = context;
  }

  public async Task<IReadOnlyCollection<Actor>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken)
  {
    int capacity = ids.Count();
    Dictionary<ActorId, Actor> actors = new(capacity);
    HashSet<Guid> missingIds = new(capacity);

    foreach (ActorId id in ids)
    {
      if (id != default)
      {
        Actor? actor = _cacheService.GetActor(id);
        if (actor == null)
        {
          missingIds.Add(id.ToGuid());
        }
        else
        {
          actors[id] = actor;
          _cacheService.SetActor(actor);
        }
      }
    }

    if (missingIds.Count > 0)
    {
      UserEntity[] users = await _context.Users.AsNoTracking()
        .Where(a => missingIds.Contains(a.Id))
        .ToArrayAsync(cancellationToken);

      foreach (UserEntity user in users)
      {
        Actor actor = Mapper.ToActor(user);
        ActorId id = new(user.Id);

        actors[id] = actor;
        _cacheService.SetActor(actor);
      }
    }

    return actors.Values;
  }
}
