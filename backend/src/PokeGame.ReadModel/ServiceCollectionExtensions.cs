using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application.Abilities;
using PokeGame.Application.Inventories;
using PokeGame.Application.Items;
using PokeGame.Application.Moves;
using PokeGame.Application.Pokedex;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Species;
using PokeGame.Application.Trainers;
using PokeGame.ReadModel.Handlers.Abilities;
using PokeGame.ReadModel.Handlers.Items;
using PokeGame.ReadModel.Handlers.Moves;
using PokeGame.ReadModel.Handlers.Pokemon;
using PokeGame.ReadModel.Handlers.Species;
using PokeGame.ReadModel.Handlers.Trainers;
using PokeGame.ReadModel.Queriers;
using System.Reflection;

namespace PokeGame.ReadModel
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
        .AddQueriers()
        .AddSynchronization()
        .AddScoped<IMappingService, MappingService>();
    }

    private static IServiceCollection AddQueriers(this IServiceCollection services)
    {
      return services
        .AddScoped<IAbilityQuerier, AbilityQuerier>()
        .AddScoped<IInventoryQuerier, InventoryQuerier>()
        .AddScoped<IItemQuerier, ItemQuerier>()
        .AddScoped<IMoveQuerier, MoveQuerier>()
        .AddScoped<IPokedexQuerier, PokedexQuerier>()
        .AddScoped<IPokemonQuerier, PokemonQuerier>()
        .AddScoped<ISpeciesQuerier, SpeciesQuerier>()
        .AddScoped<ITrainerQuerier, TrainerQuerier>();
    }

    private static IServiceCollection AddSynchronization(this IServiceCollection services)
    {
      return services
        .AddScoped<SynchronizeAbility>()
        .AddScoped<SynchronizeInventory>()
        .AddScoped<SynchronizeItem>()
        .AddScoped<SynchronizeMove>()
        .AddScoped<SynchronizePokedex>()
        .AddScoped<SynchronizePokemon>()
        .AddScoped<SynchronizeSpecies>()
        .AddScoped<SynchronizeTrainer>();
    }
  }
}
