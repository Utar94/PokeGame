using Logitar;
using PokeGame.Domain;

namespace PokeGame;

internal static class Comparisons
{
  public static bool AreEqual(string expected, UniqueName actual) => expected == actual.Value;
  public static bool AreEqual(string? expected, Description? actual) => expected?.CleanTrim() == actual?.Value;
  public static bool AreEqual(string? expected, DisplayName? actual) => expected?.CleanTrim() == actual?.Value;
  public static bool AreEqual(string? expected, Notes? actual) => expected?.CleanTrim() == actual?.Value;
  public static bool AreEqual(string? expected, Url? actual) => expected?.CleanTrim() == actual?.Value;
}
