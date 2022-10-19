using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Items;
using PokeGame.Application.Moves;
using System.Reflection;

namespace PokeGame.Application
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddPokeGameApplication(this IServiceCollection services)
    {
      Assembly assembly = typeof(ServiceCollectionExtensions).Assembly;

      return services
        .AddApplicationServices()
        .AddMediatR(assembly)
        .AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
      return services
        .AddScoped<IAbilityService, AbilityService>()
        .AddScoped<IItemService, ItemService>()
        .AddScoped<IMoveService, MoveService>();
    }
  }
}
