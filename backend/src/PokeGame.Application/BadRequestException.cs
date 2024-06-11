using Logitar.Portal.Contracts.Errors;

namespace PokeGame.Application;

public abstract class BadRequestException : Exception
{
  public abstract Error Error { get; }

  public BadRequestException() : base()
  {
  }

  public BadRequestException(string? message) : base(message)
  {
  }

  public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
