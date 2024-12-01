﻿namespace PokeGame.Application.Settings;

public record AccountSettings
{
  public const string SectionKey = "Account";

  public string DefaultTimeZone { get; set; }

  public AccountSettings() : this(string.Empty)
  {
  }

  public AccountSettings(string defaultTimeZone)
  {
    DefaultTimeZone = defaultTimeZone;
  }
}
