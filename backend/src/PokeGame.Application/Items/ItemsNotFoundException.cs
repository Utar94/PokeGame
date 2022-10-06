using System.Text;

namespace PokeGame.Application.Items
{
  public class ItemsNotFoundException : Exception
  {
    public ItemsNotFoundException(IEnumerable<Guid> ids)
      : base(GetMessage(ids))
    {
      Data["Ids"] = ids;
    }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified items could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids)}");

      return message.ToString();
    }
  }
}
