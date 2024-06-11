using Logitar.Portal.Contracts.Errors;

namespace PokeGame.Application;

public abstract class ConflictException : Exception
{
  public abstract Error Error { get; }

  public ConflictException() : base()
  {
  }

  public ConflictException(string? message) : base(message)
  {
  }

  public ConflictException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
