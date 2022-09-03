using PokeGame.Core.Items;
using System.Net;
using System.Text;

namespace PokeGame.Core.Trainers
{
  public class InsufficientQuantityException : ApiException
  {
    public InsufficientQuantityException(Item item, int missingQuantity)
      : base(HttpStatusCode.BadRequest, GetMessage(item, missingQuantity))
    {
      Item = item ?? throw new ArgumentNullException(nameof(item));
      MissingQuantity = missingQuantity;
      Value = new { code = nameof(InsufficientQuantityException).Remove(nameof(Exception)) };
    }

    public Item Item { get; }
    public int MissingQuantity { get; }

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
