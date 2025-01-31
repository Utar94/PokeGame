using Logitar.EventSourcing;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Moves;
using PokeGame.Application.Regions;
using PokeGame.Application.Speciez;

namespace PokeGame.Application;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameApplication(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcing()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddManagers();
  }

  private static IServiceCollection AddManagers(this IServiceCollection services)
  {
    return services
      .AddTransient<IAbilityManager, AbilityManager>()
      .AddTransient<IMoveManager, MoveManager>()
      .AddTransient<IRegionManager, RegionManager>()
      .AddTransient<ISpeciesManager, SpeciesManager>();
  }
}
