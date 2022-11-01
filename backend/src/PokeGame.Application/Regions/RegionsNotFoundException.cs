using System.Text;

namespace PokeGame.Application.Regions
{
  public class RegionsNotFoundException : Exception
  {
    public RegionsNotFoundException(IEnumerable<Guid> ids)
      : base(GetMessage(ids))
    {
      Data["Ids"] = ids;
    }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified regions could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids)}");

      return message.ToString();
    }
  }
}
