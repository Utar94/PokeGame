using PokeGame.Domain.Items;
using System.Text;

namespace PokeGame.Domain.Trainers
{
  public class InsufficientQuantityException : Exception
  {
    public InsufficientQuantityException(Item item, int missingQuantity)
      : base(GetMessage(item, missingQuantity))
    {
      Data["Item"] = item ?? throw new ArgumentNullException(nameof(item));
      Data["MissingQuantity"] = missingQuantity;
    }

    private static string GetMessage(Item item, int missingQuantity)
    {
      var message = new StringBuilder();

      message.AppendLine("The item's quantity is insufficient.");
      message.AppendLine($"Item: {item}");
      message.AppendLine($"Missing quantity: {missingQuantity}");

      return message.ToString();
    }
  }
}
