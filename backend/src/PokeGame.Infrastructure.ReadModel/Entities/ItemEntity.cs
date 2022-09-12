using PokeGame.Domain.Items;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class ItemEntity : Entity
  {
    public ItemCategory Category { get; set; }
    public double? DefaultModifier { get; set; }

    public int? Price { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public List<InventoryEntity> Inventory { get; set; } = new();

    public void Synchronize(Item item)
    {
      base.Synchronize(item);

      Category = item.Category;
      DefaultModifier = item.DefaultModifier;

      Price = item.Price;

      Name = item.Name;
      Description = item.Description;

      Notes = item.Notes;
      Reference = item.Reference;
    }
  }
}
