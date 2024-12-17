using Logitar.EventSourcing.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;

namespace PokeGame.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddPokeGameApplication()
      .AddSingleton<IEventSerializer, EventSerializer>()
      .AddScoped<IEventBus, EventBus>();
  }
}
