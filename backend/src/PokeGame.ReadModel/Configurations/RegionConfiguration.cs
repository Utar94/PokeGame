using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class RegionConfiguration : EntityConfiguration<RegionEntity>, IEntityTypeConfiguration<RegionEntity>
  {
    public override void Configure(EntityTypeBuilder<RegionEntity> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName("RegionId");
    }
  }
}
