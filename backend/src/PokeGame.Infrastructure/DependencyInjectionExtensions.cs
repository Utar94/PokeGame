using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PokeGame.Application.Caching;
using PokeGame.Application.Regions;
using PokeGame.Infrastructure.Actors;
using PokeGame.Infrastructure.Caching;
using PokeGame.Infrastructure.Queriers;
using PokeGame.Infrastructure.Repositories;
using PokeGame.Infrastructure.Settings;

namespace PokeGame.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .RemoveAll<IEventSerializer>()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddMemoryCache()
      .AddSingleton(InitializeCachingSettings)
      .AddSingleton<IActorService, ActorService>()
      .AddSingleton<ICacheService, CacheService>()
      .AddSingleton<IEventSerializer, EventSerializer>()
      .AddScoped<IEventBus, EventBus>()
      .AddQueriers()
      .AddRepositories();
  }

  private static CachingSettings InitializeCachingSettings(IServiceProvider serviceProvider)
  {
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetSection(CachingSettings.SectionKey).Get<CachingSettings>() ?? new();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddSingleton<IRegionQuerier, RegionQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddSingleton<IRegionRepository, RegionRepository>();
  }
}
