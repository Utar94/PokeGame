using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Configurations;

internal class RegionalSpeciesConfiguration : IEntityTypeConfiguration<RegionalSpeciesEntity>
{
  public void Configure(EntityTypeBuilder<RegionalSpeciesEntity> builder)
  {
    builder.ToTable(PokeGameDb.RegionalSpecies.Table.Table ?? string.Empty, PokeGameDb.RegionalSpecies.Table.Schema);
    builder.HasKey(x => new { x.SpeciesId, x.RegionId });

    builder.HasIndex(x => new { x.RegionId, x.Number }).IsUnique();

    builder.HasOne(x => x.Species).WithMany(x => x.RegionalSpecies)
      .HasPrincipalKey(x => x.SpeciesId).HasForeignKey(x => x.SpeciesId)
      .OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(x => x.Region).WithMany(x => x.RegionalSpecies)
      .HasPrincipalKey(x => x.RegionId).HasForeignKey(x => x.RegionId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
