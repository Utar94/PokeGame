using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Items;
using PokeGame.Application.Logging;
using PokeGame.Application.Moves;
using PokeGame.Application.Regions;
using PokeGame.EntityFrameworkCore.Actors;
using PokeGame.EntityFrameworkCore.Queriers;
using PokeGame.EntityFrameworkCore.Repositories;
using PokeGame.Infrastructure;

namespace PokeGame.EntityFrameworkCore;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameWithEntityFrameworkCore(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddPokeGameInfrastructure()
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddQueriers()
      .AddRepositories()
      .AddTransient<IActorService, ActorService>()
      .AddTransient<ILogRepository, LogRepository>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddTransient<IAbilityQuerier, AbilityQuerier>()
      .AddTransient<IItemQuerier, ItemQuerier>()
      .AddTransient<IMoveQuerier, MoveQuerier>()
      .AddTransient<IRegionQuerier, RegionQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddTransient<IAbilityRepository, AbilityRepository>()
      .AddTransient<IItemRepository, ItemRepository>()
      .AddTransient<IMoveRepository, MoveRepository>()
      .AddTransient<IRegionRepository, RegionRepository>();
  }
}
