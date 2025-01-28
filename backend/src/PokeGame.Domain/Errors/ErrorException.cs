namespace PokeGame.Domain.Errors;

public abstract class ErrorException : Exception
{
  public abstract Error Error { get; }

  protected ErrorException(string? message, Exception? innerException = null) : base(message, innerException)
  {
  }
}
