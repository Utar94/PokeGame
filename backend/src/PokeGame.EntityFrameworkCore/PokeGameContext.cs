using Microsoft.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore;

public class PokeGameContext : DbContext
{
  public PokeGameContext(DbContextOptions<PokeGameContext> options) : base(options)
  {
  }

  internal DbSet<AbilityEntity> Abilities { get; private set; }
  internal DbSet<MoveEntity> Moves { get; private set; }
  internal DbSet<RegionEntity> Regions { get; private set; }
  internal DbSet<UserEntity> Users { get; private set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
