namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class InventoryEntity
  {
    public TrainerEntity? Trainer { get; set; }
    public int TrainerId { get; set; }

    public ItemEntity? Item { get; set; }
    public int ItemId { get; set; }

    public int Quantity { get; set; }
  }
}
