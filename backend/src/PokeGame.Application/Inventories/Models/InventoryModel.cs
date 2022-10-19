using PokeGame.Application.Items.Models;

namespace PokeGame.Application.Inventories.Models
{
  public class InventoryModel
  {
    public ItemModel? Item { get; set; }

    public int Quantity { get; set; }
  }
}
