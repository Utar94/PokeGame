using Microsoft.Extensions.DependencyInjection;
using PokeGame.Core;
using PokeGame.Core.Abilities;
using PokeGame.Core.Moves;
using PokeGame.Core.Species;
using PokeGame.Infrastructure.Queriers;
using PokeGame.Infrastructure.Repositories;
using System.Reflection;

namespace PokeGame.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
    {
      Assembly assembly = typeof(ServiceCollectionExtensions).Assembly;

      return services
        .AddDbContext<PokeGameDbContext>()
        .AddQueriers()
        .AddRepositories();
    }

    private static IServiceCollection AddQueriers(this IServiceCollection services)
    {
      return services
        .AddScoped<IAbilityQuerier, AbilityQuerier>()
        .AddScoped<IMoveQuerier, MoveQuerier>()
        .AddScoped<ISpeciesQuerier, SpeciesQuerier>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
      return services
        .AddScoped<IRepository<Ability>, Repository<Ability>>()
        .AddScoped<IRepository<Move>, Repository<Move>>()
        .AddScoped<IRepository<Species>, Repository<Species>>();
    }
  }
}
