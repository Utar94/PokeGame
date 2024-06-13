namespace PokeGame.Constants;

internal static class Schemes
{
  public const string Basic = nameof(Basic);
  public const string Session = nameof(Session);

  public static string[] GetEnabled(IConfiguration configuration)
  {
    List<string> schemes = new(capacity: 4)
    {
      Session
    };

    if (configuration.GetValue<bool>("EnableBasicAuthentication"))
    {
      schemes.Add(Basic);
    }

    return [.. schemes];
  }
}
