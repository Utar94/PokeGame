using Logitar;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application;

internal static class Comparisons
{
  public static bool AreEqual(Description? description, string? value) => description?.Value == value?.CleanTrim();
  public static bool AreEqual(DisplayName? displayName, string? value) => displayName?.Value == value?.CleanTrim();
  public static bool AreEqual(Name? name, string? value) => name?.Value == value?.CleanTrim();
  public static bool AreEqual(Notes? notes, string? value) => notes?.Value == value?.CleanTrim();
  public static bool AreEqual(UniqueName? uniqueName, string? value) => uniqueName?.Value == value?.CleanTrim();
  public static bool AreEqual(Url? url, string? value) => url?.Value == value?.CleanTrim();

  public static bool AreEqual(InflictedCondition? status, InflictedConditionModel? model)
  {
    return (status == null || model == null) ? (status == null && model == null) : (status.Condition == model.Condition && status.Chance == model.Chance);
  }
}
