using Logitar.EventSourcing.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;
using PokeGame.Infrastructure.Converters;

namespace PokeGame.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingInfrastructure()
      .AddPokeGameApplication()
      .AddSingleton<IEventSerializer>(InitializeEventSerializer)
      .AddTransient<IEventBus, EventBus>();
  }

  private static EventSerializer InitializeEventSerializer(IServiceProvider serviceProvider) => new(serviceProvider.GetJsonConverters());
  public static IEnumerable<JsonConverter> GetJsonConverters(this IServiceProvider _) =>
  [
    new AbilityIdConverter(),
    new DescriptionConverter(),
    new NameConverter(),
    new NotesConverter(),
    new UrlConverter()
  ];
}
