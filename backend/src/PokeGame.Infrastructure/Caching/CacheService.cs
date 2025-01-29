using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Microsoft.Extensions.Caching.Memory;
using PokeGame.Application.Caching;
using PokeGame.Infrastructure.Settings;

namespace PokeGame.Infrastructure.Caching;

internal class CacheService : ICacheService
{
  private readonly IMemoryCache _cache;
  private readonly CachingSettings _settings;

  public CacheService(IMemoryCache cache, CachingSettings settings)
  {
    _cache = cache;
    _settings = settings;
  }

  public ActorModel? GetActor(ActorId id)
  {
    string key = GetActorKey(id);
    return TryGetValue<ActorModel>(key);
  }
  public void RemoveActor(ActorId id)
  {
    string key = GetActorKey(id);
    RemoveValue(key);
  }
  public void SetActor(ActorModel actor)
  {
    ActorId id = new(actor.Id);
    string key = GetActorKey(id);
    SetValue(key, actor, _settings.ActorLifetime);
  }
  private static string GetActorKey(ActorId id) => $"Actor.Id:{id}";

  private T? TryGetValue<T>(object key) => _cache.TryGetValue(key, out object? value) ? (T?)value : default;
  private void RemoveValue(object key) => _cache.Remove(key);
  private void SetValue<T>(object key, T value, TimeSpan duration)
  {
    _cache.Set(key, value, duration);
  }
}
