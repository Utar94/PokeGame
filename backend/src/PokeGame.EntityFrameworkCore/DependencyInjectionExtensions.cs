using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Actors;
using PokeGame.Domain.Abilities;
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
      .AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddPokeGameInfrastructure()
      .AddQueriers()
      .AddRepositories()
      .AddScoped<IActorService, ActorService>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddScoped<IAbilityQuerier, AbilityQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddScoped<IAbilityRepository, AbilityRepository>();
  }
}
