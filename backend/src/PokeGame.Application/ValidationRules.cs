namespace PokeGame.Application
{
  public static class ValidationRules
  {
    public static bool BeAValidUrl(string? s) => s == null || Uri.IsWellFormedUriString(s, UriKind.Absolute);
  }
}
