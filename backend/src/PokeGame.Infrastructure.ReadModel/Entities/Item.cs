using PokeGame.Domain;
using PokeGame.Domain.Items;
using PokeGame.Domain.Items.Payloads;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class Item : Entity
  {
    public ItemCategory Category { get; set; }

    public int? Price { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }

    public List<Inventory> Inventory { get; set; } = new();

    public void Synchronize(Domain.Items.Item item)
    {
      base.Synchronize(item);

      Category = item.Category;

      Price = item.Price;

      Name = item.Name;
      Description = item.Description;

      Notes = item.Notes;
      Reference = item.Reference;
    }
  }
}
