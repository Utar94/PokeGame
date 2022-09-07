using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Species;
using PokeGame.Domain.Trainers;

namespace PokeGame.Infrastructure
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddPokeGameInfrastructure(this IServiceCollection services)
    {
      return services
        .AddDbContext<EventContext>()
        .AddRepositories();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
      return services
        .AddScoped<IRepository<Ability>, Repository<Ability>>()
        .AddScoped<IRepository<Item>, Repository<Item>>()
        .AddScoped<IRepository<Move>, Repository<Move>>()
        .AddScoped<IRepository<Pokemon>, Repository<Pokemon>>()
        .AddScoped<IRepository<Species>, Repository<Species>>()
        .AddScoped<IRepository<Trainer>, Repository<Trainer>>();
    }
  }
}
