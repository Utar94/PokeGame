using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Core.Abilities;
using PokeGame.Core.Inventories;
using PokeGame.Core.Items;
using PokeGame.Core.Moves;
using PokeGame.Core.Species;
using PokeGame.Core.Trainers;
using System.Reflection;

namespace PokeGame.Core
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddPokeGameCore(this IServiceCollection services)
    {
      Assembly assembly = typeof(ServiceCollectionExtensions).Assembly;

      return services
        .AddAutoMapper(assembly)
        .AddValidatorsFromAssembly(assembly, includeInternalTypes: true)
        .AddScoped<IMappingService, MappingService>()
        .AddDomainServices();
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
      return services
        .AddScoped<IAbilityService, AbilityService>()
        .AddScoped<IInventoryService, InventoryService>()
        .AddScoped<IItemService, ItemService>()
        .AddScoped<IMoveService, MoveService>()
        .AddScoped<ISpeciesService, SpeciesService>()
        .AddScoped<ITrainerService, TrainerService>();
    }
  }
}
