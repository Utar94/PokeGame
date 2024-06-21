using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Contracts.Items;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class ItemCategoryConfiguration : EnumConfiguration<ItemCategoryEntity>, IEntityTypeConfiguration<ItemCategoryEntity>
{
  public override void Configure(EntityTypeBuilder<ItemCategoryEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(PokemonContext.ItemCategories));

    builder.Property(x => x.Id).HasColumnName(PokemonDb.ItemCategories.ItemCategoryId.Name);
  }

  protected override IEnumerable<ItemCategoryEntity> GetData()
  {
    return Enum.GetValues<ItemCategory>().Select(value => new ItemCategoryEntity(value));
  }
}
