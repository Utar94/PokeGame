using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Items;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Configurations
{
  internal class ItemConfiguration : EntityConfiguration<ItemEntity>, IEntityTypeConfiguration<ItemEntity>
  {
    public override void Configure(EntityTypeBuilder<ItemEntity> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Category).HasDefaultValue(default(ItemCategory));
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Picture).HasMaxLength(2048);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName("ItemId");
    }
  }
}
