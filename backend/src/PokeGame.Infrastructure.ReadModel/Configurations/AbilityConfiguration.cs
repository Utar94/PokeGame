using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
{
  internal class AbilityConfiguration : EntityConfiguration<AbilityEntity>, IEntityTypeConfiguration<AbilityEntity>
  {
    public override void Configure(EntityTypeBuilder<AbilityEntity> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName("AbilityId");
    }
  }
}
