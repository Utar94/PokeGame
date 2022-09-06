namespace PokeGame.Domain.Trainers
{
  public class InsufficientMoneyException : Exception
  {
    public InsufficientMoneyException(int missingAmount)
      : base($"The trainer is missing a money amount of {missingAmount}.")
    {
      Data["MissingAmount"] = missingAmount;
    }
  }
}
