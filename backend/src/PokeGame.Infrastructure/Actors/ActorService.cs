using Logitar.EventSourcing;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Logitar.Portal.Contracts.Users;
using PokeGame.Application.Caching;

namespace PokeGame.Infrastructure.Actors;

internal class ActorService : IActorService
{
  private readonly ICacheService _cacheService;
  private readonly IUserClient _userClient;

  public ActorService(ICacheService cacheService, IUserClient userClient)
  {
    _cacheService = cacheService;
    _userClient = userClient;
  }

  public async Task<IReadOnlyCollection<ActorModel>> FindAsync(IEnumerable<ActorId> ids, CancellationToken cancellationToken)
  {
    int capacity = ids.Count();
    Dictionary<ActorId, ActorModel> actors = new(capacity);
    HashSet<Guid> missingIds = new(capacity);

    foreach (ActorId id in ids)
    {
      ActorModel? actor = _cacheService.GetActor(id);
      if (actor == null)
      {
        missingIds.Add(id.ToGuid());
      }
      else
      {
        actors[id] = actor;
      }
    }

    if (missingIds.Count > 0)
    {
      SearchUsersPayload payload = new();
      payload.Ids.AddRange(missingIds);

      RequestContext context = new(cancellationToken);
      SearchResults<UserModel> users = await _userClient.SearchAsync(payload, context);

      foreach (UserModel user in users.Items)
      {
        ActorModel actor = new(user);
        ActorId id = new(actor.Id);
        actors[id] = actor;
      }
    }

    foreach (ActorModel actor in actors.Values)
    {
      _cacheService.SetActor(actor);
    }

    return actors.Values;
  }
}
