using PokeGame.Domain;

namespace PokeGame.Infrastructure.PokeGameDb;

internal static class Helper
{
  public static string Normalize(UniqueName uniqueName) => Normalize(uniqueName.Value);
  public static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
