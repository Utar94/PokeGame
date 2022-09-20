using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;

namespace PokeGame.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
    {
      return services
        .AddDbContext<EventContext>()
        .AddScoped<IRepository, Repository>();
    }
  }
}
