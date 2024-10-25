﻿namespace PokeGame.Settings;

internal record RefreshTokenCookieSettings
{
  public bool HttpOnly { get; set; } = true;
  public TimeSpan? MaxAge { get; set; } = TimeSpan.FromDays(7);
  public SameSiteMode SameSite { get; set; } = SameSiteMode.Strict;
  public bool Secure { get; set; } = true;
}
