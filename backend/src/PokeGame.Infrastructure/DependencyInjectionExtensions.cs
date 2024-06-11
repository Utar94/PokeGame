using Logitar.EventSourcing.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;
using PokeGame.Application.Caching;
using PokeGame.Infrastructure.Caching;
using PokeGame.Infrastructure.Converters;
using PokeGame.Infrastructure.Settings;

namespace PokeGame.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingInfrastructure()
      .AddMemoryCache()
      .AddPokeGameApplication()
      .AddSingleton(InitializeCachingSettings)
      .AddSingleton<ICacheService, CacheService>()
      .AddSingleton<IEventSerializer>(InitializeEventSerializer())
      .AddTransient<IEventBus, EventBus>();
  }

  private static CachingSettings InitializeCachingSettings(IServiceProvider serviceProvider)
  {
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetSection("Caching").Get<CachingSettings>() ?? new();
  }

  private static EventSerializer InitializeEventSerializer() => new(
  [
    new AbilityIdConverter(),
    new DescriptionConverter(),
    new DisplayNameConverter(),
    new NotesConverter(),
    new ReferenceConverter(),
    new UniqueNameConverter()
  ]);
}
