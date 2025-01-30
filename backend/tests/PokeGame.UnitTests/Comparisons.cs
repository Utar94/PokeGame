using Logitar;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame;

internal static class Comparisons
{
  public static bool AreEqual(string expected, UniqueName actual) => expected == actual.Value;
  public static bool AreEqual(string? expected, Description? actual) => expected?.CleanTrim() == actual?.Value;
  public static bool AreEqual(string? expected, DisplayName? actual) => expected?.CleanTrim() == actual?.Value;
  public static bool AreEqual(string? expected, Notes? actual) => expected?.CleanTrim() == actual?.Value;
  public static bool AreEqual(string? expected, Url? actual) => expected?.CleanTrim() == actual?.Value;

  public static bool AreEqual(int? expected, Accuracy? actual) => expected == actual?.Value;
  public static bool AreEqual(int? expected, Power? actual) => expected == actual?.Value;
  public static bool AreEqual(int expected, PowerPoints actual) => expected == actual.Value;
  public static bool AreEqual(IInflictedStatus? expected, IInflictedStatus? actual)
  {
    if (expected == null || actual == null)
    {
      return expected == null && actual == null;
    }

    return expected.Condition == actual.Condition && expected.Chance == actual.Chance;
  }
}
