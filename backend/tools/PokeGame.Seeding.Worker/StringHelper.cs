namespace PokeGame.Seeding.Worker;

internal static class StringHelper
{
  public static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
