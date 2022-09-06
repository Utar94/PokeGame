using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Inventories;
using PokeGame.Application.Items;
using PokeGame.Application.Moves;
using PokeGame.Application.Species;
using PokeGame.Application.Trainers;
using PokeGame.Infrastructure.ReadModel.Queriers;
using System.Reflection;

namespace PokeGame.Infrastructure.ReadModel
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddPokeGameReadModel(this IServiceCollection services)
    {
      Assembly assembly = typeof(ServiceCollectionExtensions).Assembly;

      return services
        .AddAutoMapper(assembly)
        .AddDbContext<ReadContext>()
        .AddMediatR(assembly)
        .AddQueriers();
    }

    private static IServiceCollection AddQueriers(this IServiceCollection services)
    {
      return services
        .AddScoped<IAbilityQuerier, AbilityQuerier>()
        .AddScoped<IInventoryQuerier, InventoryQuerier>()
        .AddScoped<IItemQuerier, ItemQuerier>()
        .AddScoped<IMoveQuerier, MoveQuerier>()
        .AddScoped<ISpeciesQuerier, SpeciesQuerier>()
        .AddScoped<ITrainerQuerier, TrainerQuerier>();
    }
  }
}
