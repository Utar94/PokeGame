using System.Text;

namespace PokeGame.Domain.Pokemon
{
  public class NatureNotFoundException : Exception
  {
    public NatureNotFoundException(string name, string? paramName)
      : base(GetMessage(name, paramName))
    {
      Data["Name"] = name ?? throw new ArgumentNullException(nameof(name));

      if (paramName != null)
        Data["ParamName"] = paramName;
    }

    private static string GetMessage(string name, string? paramName)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified nature could not be found.");
      message.AppendLine($"Name: {name}");

      if (paramName != null)
        message.AppendLine($"ParamName: {paramName}");

      return message.ToString();
    }
  }
}
