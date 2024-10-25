namespace PokeGame.Settings;

internal record OpenAuthenticationSettings
{
  public const string SectionKey = "OpenAuthentication";

  public AccessTokenSettings AccessToken { get; set; } = new();
}
