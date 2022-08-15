using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Core.Abilities;
using PokeGame.Core.Moves;
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
        .AddScoped<IMoveService, MoveService>()
        .AddScoped<IAbilityService, AbilityService>();
    }
  }
}
