﻿namespace PokeGame.Contracts.Accounts;

public record SignInPayload
{
  public string Locale { get; set; }

  public Credentials? Credentials { get; set; }
  public string? AuthenticationToken { get; set; }
  public string? GoogleIdToken { get; set; }
  public OneTimePasswordPayload? OneTimePassword { get; set; }
  public CompleteProfilePayload? Profile { get; set; }

  public SignInPayload() : this(string.Empty)
  {
  }

  public SignInPayload(string locale)
  {
    Locale = locale;
  }
}
