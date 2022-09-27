using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
{
  internal class RegionalSpeciesConfiguration : IEntityTypeConfiguration<RegionalSpeciesEntity>
  {
    public void Configure(EntityTypeBuilder<RegionalSpeciesEntity> builder)
    {
      builder.HasKey(x => new { x.SpeciesId, x.Region });
      builder.HasIndex(x => new { x.Region, x.Number }).IsUnique();
    }
  }
}
