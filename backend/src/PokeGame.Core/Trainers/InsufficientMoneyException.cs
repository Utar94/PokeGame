using System.Net;

namespace PokeGame.Core.Trainers
{
  public class InsufficientMoneyException : ApiException
  {
    public InsufficientMoneyException(int missingAmount)
      : base(HttpStatusCode.BadRequest, $"The trainer is missing a money amount of {missingAmount}.")
    {
      MissingAmount = missingAmount;
      Value = new { code = nameof(InsufficientMoneyException).Remove(nameof(Exception)) };
    }

    public int MissingAmount { get; }
  }
}
