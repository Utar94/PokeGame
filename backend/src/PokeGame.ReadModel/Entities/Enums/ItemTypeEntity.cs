using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Items;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class ItemTypeEntity : EnumEntity
  {
    public ItemTypeEntity(ItemType itemType) : base(itemType)
    {
    }
    private ItemTypeEntity() : base()
    {
    }
  }

  internal class ItemTypeEntityConfiguration : EnumEntityConfiguration<ItemType, ItemTypeEntity>
  {
    public override void Configure(EntityTypeBuilder<ItemTypeEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("ItemTypes");
    }

    protected override IEnumerable<ItemTypeEntity> GetData()
      => Enum.GetValues<ItemType>().Select(value => new ItemTypeEntity(value));
  }
}
