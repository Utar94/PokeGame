using Logitar.EventSourcing;

namespace PokeGame.Application;

public interface IApplicationContext
{
  ActorId? GetActorId();
}
