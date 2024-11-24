using Logitar.EventSourcing.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;
using PokeGame.Application.Accounts;
using PokeGame.Application.Caching;
using PokeGame.Infrastructure.Caching;
using PokeGame.Infrastructure.Converters;
using PokeGame.Infrastructure.IdentityServices;
using PokeGame.Infrastructure.Settings;

namespace PokeGame.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingInfrastructure()
      .AddIdentityServices()
      .AddMemoryCache()
      .AddPokeGameApplication()
      .AddSingleton(InitializeCachingSettings)
      .AddSingleton<ICacheService, CacheService>()
      .AddSingleton<IEventSerializer>(InitializeEventSerializer)
      .AddTransient<IEventBus, EventBus>();
  }

  private static IServiceCollection AddIdentityServices(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyService, ApiKeyService>()
      .AddTransient<IGoogleService, GoogleService>()
      .AddTransient<IMessageService, MessageService>()
      .AddTransient<IOneTimePasswordService, OneTimePasswordService>()
      .AddTransient<IRealmService, RealmService>()
      .AddTransient<ISessionService, SessionService>()
      .AddTransient<ITokenService, TokenService>()
      .AddTransient<IUserService, UserService>();
  }

  private static CachingSettings InitializeCachingSettings(IServiceProvider serviceProvider)
  {
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetSection(CachingSettings.SectionKey).Get<CachingSettings>() ?? new();
  }

  private static EventSerializer InitializeEventSerializer(IServiceProvider serviceProvider) => new(serviceProvider.GetJsonConverters());
  public static IEnumerable<JsonConverter> GetJsonConverters(this IServiceProvider _) =>
  [
    new AbilityIdConverter(),
    new DescriptionConverter(),
    new DisplayNameConverter(),
    new MoveIdConverter(),
    new NotesConverter(),
    new RegionIdConverter(),
    new SpeciesIdConverter(),
    new UniqueNameConverter(),
    new UrlConverter(),
    new VolatileConditionConverter()
  ];
}
