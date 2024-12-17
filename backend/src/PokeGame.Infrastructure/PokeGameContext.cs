using Microsoft.EntityFrameworkCore;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure;

public class PokeGameContext : DbContext
{
  public PokeGameContext(DbContextOptions<PokeGameContext> options) : base(options)
  {
  }

  internal DbSet<RegionEntity> Regions => Set<RegionEntity>();
  internal DbSet<UserEntity> Users => Set<UserEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
