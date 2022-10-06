namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class InventoryEntity
  {
    public InventoryEntity(TrainerEntity trainer, ItemEntity item)
    {
      Trainer = trainer;
      TrainerId = trainer.Sid;
      Item = item;
      ItemId = item.Sid;
    }
    private InventoryEntity()
    {
    }

    public TrainerEntity? Trainer { get; private set; }
    public int TrainerId { get; private set; }

    public ItemEntity? Item { get; private set; }
    public int ItemId { get; private set; }

    public int Quantity { get; set; }
  }
}
