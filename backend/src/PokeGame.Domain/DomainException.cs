using Logitar.Portal.Contracts.Errors;

namespace PokeGame.Domain;

public abstract class DomainException : Exception
{
  public abstract Error Error { get; }

  protected DomainException() : base()
  {
  }

  protected DomainException(string? message) : base(message)
  {
  }

  protected DomainException(Exception? innerException) : this(message: null, innerException)
  {
  }

  protected DomainException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
