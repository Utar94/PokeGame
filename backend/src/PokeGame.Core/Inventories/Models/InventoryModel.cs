using PokeGame.Core.Items.Models;

namespace PokeGame.Core.Inventories.Models
{
  public class InventoryModel
  {
    public ItemSummary? Item { get; set; }

    public int Quantity { get; set; }
  }
}
