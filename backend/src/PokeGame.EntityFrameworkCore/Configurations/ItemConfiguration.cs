using Logitar.Identity.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class ItemConfiguration : AggregateConfiguration<ItemEntity>, IEntityTypeConfiguration<ItemEntity>
{
  public override void Configure(EntityTypeBuilder<ItemEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(PokemonContext.Items));
    builder.HasKey(x => x.ItemId);

    builder.HasIndex(x => x.Category);
    builder.HasIndex(x => x.Price);
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.UniqueNameNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Ignore(x => x.Properties);

    builder.Property(x => x.UniqueName).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(UniqueNameUnit.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayNameUnit.MaximumLength);
    builder.Property(x => x.Picture).HasMaxLength(UrlUnit.MaximumLength);
    builder.Property(x => x.Reference).HasMaxLength(UrlUnit.MaximumLength);
    builder.Property(x => x.PropertiesSerialized).HasColumnName(nameof(ItemEntity.Properties));
  }
}
