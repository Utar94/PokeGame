using PokeGame.Core.Items;
using PokeGame.Core.Trainers;

namespace PokeGame.Core.Inventories
{
  public class Inventory
  {
    public Inventory(Trainer trainer, Item item)
    {
      Trainer = trainer ?? throw new ArgumentNullException(nameof(trainer));
      TrainerId = trainer.Sid;
      Item = item ?? throw new ArgumentNullException(nameof(item));
      ItemId = item.Sid;
    }
    private Inventory()
    {
    }

    public Trainer? Trainer { get; private set; }
    public int TrainerId { get; private set; }

    public Item? Item { get; private set; }
    public int ItemId { get; private set; }

    public int Quantity { get; set; }
  }
}
