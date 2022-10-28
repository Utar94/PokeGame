using PokeGame.Domain.Items;

namespace PokeGame.ReadModel.Entities
{
  internal class ItemEntity : Entity
  {
    public ItemCategory Category { get; private set; }
    public ItemKind? Kind { get; private set; }
    public double? DefaultModifier { get; private set; }

    public int? Price { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Picture { get; private set; }
    public string? Reference { get; private set; }

    public List<EvolutionEntity> Evolutions { get; private set; } = new();
    public List<InventoryEntity> Inventory { get; private set; } = new();

    public void Synchronize(Item item)
    {
      base.Synchronize(item);

      Category = item.Category;
      Kind = item.Kind;
      DefaultModifier = item.DefaultModifier;

      Price = item.Price;

      Name = item.Name;
      Description = item.Description;

      Notes = item.Notes;
      Picture = item.Picture;
      Reference = item.Reference;
    }
  }
}
