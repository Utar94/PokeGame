namespace PokeGame.Domain
{
  public static class StringExtensions
  {
    public static string? CleanTrim(this string? s)
    {
      return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    }
  }
}
