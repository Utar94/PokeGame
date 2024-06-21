using Microsoft.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore;

public class PokemonContext : DbContext
{
  public PokemonContext(DbContextOptions<PokemonContext> options) : base(options)
  {
  }

  internal DbSet<AbilityEntity> Abilities { get; private set; }
  internal DbSet<ActorEntity> Actors { get; private set; }
  internal DbSet<ItemCategoryEntity> ItemCategories { get; private set; }
  internal DbSet<ItemEntity> Items { get; private set; }
  internal DbSet<LogEventEntity> LogEvents { get; private set; }
  internal DbSet<LogExceptionEntity> LogExceptions { get; private set; }
  internal DbSet<LogEntity> Logs { get; private set; }
  internal DbSet<MoveCategoryEntity> MoveCategories { get; private set; }
  internal DbSet<MoveEntity> Moves { get; private set; }
  internal DbSet<PokemonTypeEntity> PokemonTypes { get; private set; }
  internal DbSet<RegionEntity> Regions { get; private set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
