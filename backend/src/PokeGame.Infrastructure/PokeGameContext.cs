using Microsoft.EntityFrameworkCore;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure;

public class PokeGameContext : DbContext
{
  public PokeGameContext(DbContextOptions<PokeGameContext> options) : base(options)
  {
  }

  internal DbSet<AbilityEntity> Abilities => Set<AbilityEntity>();
  internal DbSet<MoveEntity> Moves => Set<MoveEntity>();
  internal DbSet<RegionalSpeciesEntity> RegionalSpecies => Set<RegionalSpeciesEntity>();
  internal DbSet<RegionEntity> Regions => Set<RegionEntity>();
  internal DbSet<SpeciesEntity> Species => Set<SpeciesEntity>();
  internal DbSet<UserEntity> Users => Set<UserEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
