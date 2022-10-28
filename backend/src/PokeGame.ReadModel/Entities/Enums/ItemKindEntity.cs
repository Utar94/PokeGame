using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Items;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class ItemKindEntity : EnumEntity
  {
    public ItemKindEntity(ItemKind itemType) : base(itemType)
    {
    }
    private ItemKindEntity() : base()
    {
    }
  }

  internal class ItemKindEntityConfiguration : EnumEntityConfiguration<ItemKind, ItemKindEntity>
  {
    public override void Configure(EntityTypeBuilder<ItemKindEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("ItemKinds");
    }

    protected override IEnumerable<ItemKindEntity> GetData()
      => Enum.GetValues<ItemKind>().Select(value => new ItemKindEntity(value));
  }
}
