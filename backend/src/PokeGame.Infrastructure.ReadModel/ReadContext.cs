using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel
{
  public class ReadContext : DbContext
  {
    private readonly IConfiguration _configuration;

    public ReadContext(IConfiguration configuration, DbContextOptions<ReadContext> options) : base(options)
    {
      _configuration = configuration;
    }

    internal DbSet<Ability> Abilities { get; private set; } = null!;
    internal DbSet<Inventory> Inventory { get; private set; } = null!;
    internal DbSet<Item> Items { get; private set; } = null!;
    internal DbSet<Move> Moves { get; private set; } = null!;
    internal DbSet<Species> Species { get; private set; } = null!;
    internal DbSet<Trainer> Trainers { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_configuration.GetValue<string>($"POSTGRESQLCONNSTR_{nameof(ReadContext)}"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(typeof(ReadContext).Assembly);

      builder.HasDefaultSchema("ReadModel");

      builder.HasPostgresExtension("uuid-ossp");
    }
  }
}
