using Logitar;
using PokeGame.Domain;

namespace PokeGame.Application;

internal static class Comparisons
{
  public static bool AreEqual(Description? description, string? value) => description?.Value == value?.CleanTrim();
  public static bool AreEqual(Name? name, string? value) => name?.Value == value?.CleanTrim();
  public static bool AreEqual(Notes? notes, string? value) => notes?.Value == value?.CleanTrim();
  public static bool AreEqual(Url? url, string? value) => url?.Value == value?.CleanTrim();
}
