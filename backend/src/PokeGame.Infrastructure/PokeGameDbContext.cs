using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokeGame.Core;
using PokeGame.Core.Abilities;
using PokeGame.Core.Items;
using PokeGame.Core.Moves;
using PokeGame.Core.Species;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure
{
  public class PokeGameDbContext : DbContext
  {
    private readonly IConfiguration _configuration;

    public PokeGameDbContext(IConfiguration configuration, DbContextOptions<PokeGameDbContext> options)
      : base(options)
    {
      _configuration = configuration;
    }

    public DbSet<Ability> Abilities { get; private set; } = null!;
    public DbSet<Event> Events { get; private set; } = null!;
    public DbSet<Item> Items { get; private set; } = null!;
    public DbSet<Move> Moves { get; private set; } = null!;
    public DbSet<Species> Species { get; private set; } = null!;

    internal DbSet<SpeciesAbility> SpeciesAbilities { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_configuration.GetValue<string>($"POSTGRESQLCONNSTR_{nameof(PokeGameDbContext)}"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(typeof(PokeGameDbContext).Assembly);

      builder.Ignore<EventBase>();

      builder.HasPostgresExtension("uuid-ossp");
    }
  }
}
