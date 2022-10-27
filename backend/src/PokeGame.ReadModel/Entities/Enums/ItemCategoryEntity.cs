using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Items;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class ItemCategoryEntity : EnumEntity
  {
    public ItemCategoryEntity(ItemCategory itemCategory) : base(itemCategory)
    {
    }
    private ItemCategoryEntity() : base()
    {
    }
  }

  internal class ItemCategoryEntityConfiguration : EnumEntityConfiguration<ItemCategory, ItemCategoryEntity>
  {
    public override void Configure(EntityTypeBuilder<ItemCategoryEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("ItemCategories");
    }

    protected override IEnumerable<ItemCategoryEntity> GetData()
      => Enum.GetValues<ItemCategory>().Select(value => new ItemCategoryEntity(value));
  }
}
