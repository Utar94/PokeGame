namespace PokeGame.Settings;

internal record OpenAuthenticationSettings
{
  public AccessTokenSettings AccessToken { get; set; } = new();
}
