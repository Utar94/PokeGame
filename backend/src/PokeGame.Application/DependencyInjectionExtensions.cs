using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Logging;

namespace PokeGame.Application;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameApplication(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddScoped<ILoggingService, LoggingService>()
      .AddTransient<IRequestPipeline, RequestPipeline>();
  }
}
