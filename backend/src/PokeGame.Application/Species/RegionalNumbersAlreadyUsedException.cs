using System.Text;

namespace PokeGame.Application.Species
{
  public class RegionalNumbersAlreadyUsedException : Exception
  {
    public RegionalNumbersAlreadyUsedException(IEnumerable<KeyValuePair<Guid, int>> regionalNumbers, string paramName)
      : base(GetMessage(regionalNumbers, paramName))
    {
      Data["RegionalNumbers"] = new Dictionary<Guid, int>(regionalNumbers);
      Data["ParamName"] = paramName;
    }

    private static string GetMessage(IEnumerable<KeyValuePair<Guid, int>> regionalNumbers, string paramName)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified regional numbers are already used.");

      if (regionalNumbers.Any())
      {
        message.AppendLine("Regional Numbers:");
        foreach (KeyValuePair<Guid, int> pair in regionalNumbers)
        {
          message.AppendLine($"{pair.Key}: {pair.Value}");
        }
      }

      message.AppendLine($"ParamName: {paramName}");

      return message.ToString();
    }
  }
}
