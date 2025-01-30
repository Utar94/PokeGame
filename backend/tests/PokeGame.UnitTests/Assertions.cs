using Logitar;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame;

internal static class Assertions
{
  public static void Equal(string expected, UniqueName actual)
  {
    Assert.Equal(expected, actual.Value);
  }
  public static void Equal(string? expected, Description? actual)
  {
    Assert.Equal(expected?.CleanTrim(), actual?.Value);
  }
  public static void Equal(string? expected, DisplayName? actual)
  {
    Assert.Equal(expected?.CleanTrim(), actual?.Value);
  }
  public static void Equal(string? expected, Notes? actual)
  {
    Assert.Equal(expected?.CleanTrim(), actual?.Value);
  }
  public static void Equal(string? expected, Url? actual)
  {
    Assert.Equal(expected?.CleanTrim(), actual?.Value);
  }

  public static void Equal(int? expected, Accuracy? actual)
  {
    Assert.Equal(expected, actual?.Value);
  }
  public static void Equal(int? expected, Power? actual)
  {
    Assert.Equal(expected, actual?.Value);
  }
  public static void Equal(int expected, PowerPoints actual)
  {
    Assert.Equal(expected, actual.Value);
  }
  public static void Equal(IInflictedStatus? expected, IInflictedStatus? actual)
  {
    if (expected == null || actual == null)
    {
      Assert.Null(expected);
      Assert.Null(actual);
    }
    else
    {
      Assert.Equal(expected.Condition, actual.Condition);
      Assert.Equal(expected.Chance, actual.Chance);
    }
  }
}
