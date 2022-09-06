using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokeGame.Infrastructure.Configurations;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure
{
  public class EventContext : DbContext
  {
    private readonly IConfiguration _configuration;

    public EventContext(IConfiguration configuration, DbContextOptions<EventContext> options) : base(options)
    {
      _configuration = configuration;
    }

    internal DbSet<Event> Events { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_configuration.GetValue<string>($"POSTGRESQLCONNSTR_{nameof(EventContext)}"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(typeof(EventContext).Assembly);

      builder.HasDefaultSchema("Event");

      builder.HasPostgresExtension("uuid-ossp");
    }
  }
}
