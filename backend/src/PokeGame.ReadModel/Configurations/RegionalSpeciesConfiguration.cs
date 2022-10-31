using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class RegionalSpeciesConfiguration : IEntityTypeConfiguration<RegionalSpeciesEntity>
  {
    public void Configure(EntityTypeBuilder<RegionalSpeciesEntity> builder)
    {
      builder.HasKey(x => new { x.SpeciesId, x.RegionId });
      builder.HasIndex(x => new { x.RegionId, x.Number }).IsUnique();
    }
  }
}
