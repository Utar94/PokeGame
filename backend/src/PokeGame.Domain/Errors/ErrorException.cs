namespace PokeGame.Domain.Errors;

public abstract class ErrorException : Exception
{
  public abstract Error Error { get; }

  public ErrorException(string? message, Exception? innerException = null) : base(message, innerException)
  {
  }
}
