using Microsoft.Extensions.DependencyInjection;

namespace PokeGame.Application;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameApplication(this IServiceCollection services)
  {
    return services
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddTransient<IRequestPipeline, RequestPipeline>();
  }
}
