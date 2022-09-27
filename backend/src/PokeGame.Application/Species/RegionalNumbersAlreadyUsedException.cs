using PokeGame.Domain;
using System.Text;

namespace PokeGame.Application.Species
{
  public class RegionalNumbersAlreadyUsedException : Exception
  {
    public RegionalNumbersAlreadyUsedException(IEnumerable<KeyValuePair<Region, int>> regionalNumbers, string paramName)
      : base(GetMessage(regionalNumbers, paramName))
    {
      ArgumentNullException.ThrowIfNull(regionalNumbers);

      Data["RegionalNumbers"] = new Dictionary<Region, int>(regionalNumbers);
      Data["ParamName"] = paramName ?? throw new ArgumentNullException(nameof(paramName));
    }

    private static string GetMessage(IEnumerable<KeyValuePair<Region, int>> regionalNumbers, string paramName)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified regional numbers are already used.");

      if (regionalNumbers?.Any() == true)
      {
        message.AppendLine("Regional Numbers:");
        foreach (KeyValuePair<Region, int> pair in regionalNumbers)
        {
          message.AppendLine($"{pair.Key}: {pair.Value}");
        }
      }

      if (paramName != null)
      {
        message.AppendLine($"ParamName: {paramName}");
      }

      return message.ToString();
    }
  }
}
