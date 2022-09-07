using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Inventories;
using PokeGame.Application.Items;
using PokeGame.Application.Moves;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Species;
using PokeGame.Application.Trainers;
using System.Reflection;

namespace PokeGame.Application
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddPokeGameApplication(this IServiceCollection services)
    {
      Assembly assembly = typeof(ServiceCollectionExtensions).Assembly;

      return services
        .AddValidatorsFromAssembly(assembly, includeInternalTypes: true)
        .AddApplicationServices();
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
      return services
        .AddScoped<IAbilityService, AbilityService>()
        .AddScoped<IInventoryService, InventoryService>()
        .AddScoped<IItemService, ItemService>()
        .AddScoped<IMoveService, MoveService>()
        .AddScoped<IPokemonService, PokemonService>()
        .AddScoped<ISpeciesService, SpeciesService>()
        .AddScoped<ITrainerService, TrainerService>();
    }
  }
}
