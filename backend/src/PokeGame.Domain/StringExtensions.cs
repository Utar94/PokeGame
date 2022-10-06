namespace PokeGame.Domain
{
  internal static class StringExtensions
  {
    public static byte Checksum(this string s)
    {
      int sum = 0;

      char[] chars = s.ToCharArray();
      foreach (char c in chars)
      {
        sum += char.IsDigit(c) ? (c - '0') : c.ToString().Checksum();
      }

      return sum < 10 ? (byte)sum : sum.ToString().Checksum();
    }

    public static string? CleanTrim(this string? s)
    {
      return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    }
  }
}
