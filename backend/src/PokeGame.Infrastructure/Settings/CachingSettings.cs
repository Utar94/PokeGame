namespace PokeGame.Infrastructure.Settings;

internal record CachingSettings
{
  public TimeSpan? ActorLifetime { get; set; }
}
