using PokeGame.Contracts.Items;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class ItemCategoryEntity : EnumEntity
{
  public ItemCategoryEntity(ItemCategory moveCategory) : base(moveCategory)
  {
  }

  private ItemCategoryEntity() : base()
  {
  }
}
