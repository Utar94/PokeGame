﻿using Logitar.Portal.Contracts.Errors;

namespace PokeGame.Contracts.Errors;

public record InvalidCredentialsError : Error
{
  public InvalidCredentialsError() : this("InvalidCredentials", "The specified credentials did not match.")
  {
  }

  public InvalidCredentialsError(string code, string message) : base(code, message)
  {
  }
}
