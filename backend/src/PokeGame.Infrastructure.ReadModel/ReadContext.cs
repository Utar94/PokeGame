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

    internal DbSet<AbilityEntity> Abilities { get; private set; } = null!;
    internal DbSet<InventoryEntity> Inventory { get; private set; } = null!;
    internal DbSet<ItemEntity> Items { get; private set; } = null!;
    internal DbSet<MoveEntity> Moves { get; private set; } = null!;
    internal DbSet<PokedexEntity> Pokedex { get; private set; } = null!;
    internal DbSet<PokemonEntity> Pokemon { get; private set; } = null!;
    internal DbSet<PokemonMoveEntity> PokemonMoves { get; private set; } = null!;
    internal DbSet<SpeciesEntity> Species { get; private set; } = null!;
    internal DbSet<SpeciesAbilityEntity> SpeciesAbilities { get; private set; } = null!;
    internal DbSet<TrainerEntity> Trainers { get; private set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_configuration.GetValue<string>($"POSTGRESQLCONNSTR_{nameof(ReadContext)}"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(typeof(ReadContext).Assembly);

      builder.Entity<SpeciesAbilityEntity>().HasKey(x => new { x.SpeciesId, x.AbilityId });

      builder.HasDefaultSchema("ReadModel");

      builder.HasPostgresExtension("uuid-ossp");
    }
  }
}
