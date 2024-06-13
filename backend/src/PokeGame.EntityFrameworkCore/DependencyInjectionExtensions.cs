using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Logging;
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
      .AddTransient<IActorService, ActorService>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services.AddTransient<IAbilityQuerier, AbilityQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddTransient<IAbilityRepository, AbilityRepository>()
      .AddTransient<ILogRepository, LogRepository>();
  }
}
