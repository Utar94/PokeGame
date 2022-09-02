namespace PokeGame.Core
{
  public static class StringExtensions
  {
    public static byte Checksum(this string s)
    {
      ArgumentNullException.ThrowIfNull(s);

      int sum = 0;

      char[] chars = s.ToCharArray();
      foreach (char c in chars)
      {
        sum += char.IsDigit(c) ? (c - '0') : c.ToString().Checksum();
      }

      return sum < 10 ? (byte)sum : sum.ToString().Checksum();
    }

    public static string? CleanTrim(this string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    public static string Remove(this string s, string pattern)
      => s?.Replace(pattern, string.Empty) ?? throw new ArgumentNullException(nameof(s));
  }
}
