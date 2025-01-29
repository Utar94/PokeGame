using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;

namespace PokeGame.Application.Caching;

public interface ICacheService
{
  ActorModel? GetActor(ActorId id);
  void RemoveActor(ActorId id);
  void SetActor(ActorModel actor);
}
