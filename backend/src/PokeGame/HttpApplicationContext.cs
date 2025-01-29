using Logitar.EventSourcing;
using PokeGame.Application;

namespace PokeGame;

internal class HttpApplicationContext : IApplicationContext
{
  public ActorId? GetActorId() => null; // TODO(fpion): implement
}
