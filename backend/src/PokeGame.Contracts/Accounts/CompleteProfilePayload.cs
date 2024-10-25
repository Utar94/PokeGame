﻿namespace PokeGame.Contracts.Accounts;

public record CompleteProfilePayload : SaveProfilePayload
{
  public string Token { get; set; }

  public string? Password { get; set; }

  public CompleteProfilePayload() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
  {
  }

  public CompleteProfilePayload(string token, string firstName, string lastName, string locale, string timeZone)
    : base(firstName, lastName, locale, timeZone)
  {
    Token = token;
  }
}
