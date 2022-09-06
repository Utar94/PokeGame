namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class Inventory
  {
    public Trainer? Trainer { get; set; }
    public int TrainerId { get; set; }

    public Item? Item { get; set; }
    public int ItemId { get; set; }

    public int Quantity { get; set; }
  }
}
