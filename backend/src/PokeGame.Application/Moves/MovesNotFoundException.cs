using System.Text;

namespace PokeGame.Application.Moves
{
  public class MovesNotFoundException : Exception
  {
    public MovesNotFoundException(IEnumerable<Guid> ids)
      : base(GetMessage(ids))
    {
      Data["Ids"] = ids;
    }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified moves could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids)}");

      return message.ToString();
    }
  }
}
