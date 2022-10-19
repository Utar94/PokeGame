using System.Text;

namespace PokeGame.Application.Inventories
{
  public class InventoryNotFoundException : Exception
  {
    public InventoryNotFoundException(Guid trainerId, Guid itemId)
      : base(GetMessage(trainerId, itemId))
    {
      Data["TrainerId"] = trainerId;
      Data["ItemId"] = itemId;
    }

    private static string GetMessage(Guid trainerId, Guid itemId)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified inventory could not be found.");
      message.AppendLine($"TrainerId: {trainerId}");
      message.AppendLine($"ItemId: {itemId}");

      return message.ToString();
    }
  }
}
