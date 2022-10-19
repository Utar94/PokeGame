using System.Text;

namespace PokeGame.Application.Species
{
  public class SpeciesNumberAlreadyUsedException : Exception
  {
    public SpeciesNumberAlreadyUsedException(int number, string paramName)
      : base(GetMessage(number, paramName))
    {
      Data["Number"] = number;
      Data["ParamName"] = paramName;
    }

    private static string GetMessage(int number, string paramName)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified species number is already used.");
      message.AppendLine($"Number: {number}");
      message.AppendLine($"ParamName: {paramName}");

      return message.ToString();
    }
  }
}
