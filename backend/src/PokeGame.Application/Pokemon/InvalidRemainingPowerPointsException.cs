using PokeGame.Domain.Moves;
using System.Text;

namespace PokeGame.Application.Pokemon
{
  public class RemainingPowerPointsExceededException : Exception
  {
    public RemainingPowerPointsExceededException(Move move, byte remainingPowerPoints)
      : base(GetMessage(move, remainingPowerPoints))
    {
      Data["MoveId"] = move.Id;
      Data["RemainingPowerPoints"] = remainingPowerPoints;
    }

    public static string GetMessage(Move move, byte remainingPowerPoints)
    {
      var message = new StringBuilder();

      message.AppendLine("The remaining power points exceed the move power points.");
      message.AppendLine($"Move: {move} (PP={move.PowerPoints})");
      message.AppendLine($"Remaining power points: {remainingPowerPoints}");

      return message.ToString();
    }
  }
}
