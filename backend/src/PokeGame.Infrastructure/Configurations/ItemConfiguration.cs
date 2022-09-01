using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Core.Items;

namespace PokeGame.Infrastructure.Configurations
{
  internal class ItemConfiguration : AggregateConfiguration<Item>, IEntityTypeConfiguration<Item>
  {
    public override void Configure(EntityTypeBuilder<Item> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Category).HasDefaultValue(default(ItemCategory));
      builder.Property(x => x.Name).HasMaxLength(100);
      builder.Property(x => x.Reference).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName($"{nameof(Item)}Id");
    }
  }
}
