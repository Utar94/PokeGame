namespace PokeGame.Application
{
  internal static class ValidationRules
  {
    public static bool BeAValidUrl(string? s) => s == null || Uri.IsWellFormedUriString(s, UriKind.Absolute);
  }
}
