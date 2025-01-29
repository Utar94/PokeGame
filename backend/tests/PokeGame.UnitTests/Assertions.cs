using Logitar;
using PokeGame.Domain;

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
}
