using System.Text;

namespace PokeGame.Application.Species
{
  public class AbilitiesNotFoundException : Exception
  {
    public AbilitiesNotFoundException(IEnumerable<Guid> ids)
      : base(GetMessage(ids))
    {
      Data["Ids"] = ids;
    }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified abilities could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids)}");

      return message.ToString();
    }
  }
}
