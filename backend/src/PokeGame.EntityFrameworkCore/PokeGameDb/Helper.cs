namespace PokeGame.EntityFrameworkCore.PokeGameDb;

internal static class Helper
{
  public static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
