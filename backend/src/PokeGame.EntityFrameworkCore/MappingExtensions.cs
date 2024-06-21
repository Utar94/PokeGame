using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore;

internal static class MappingExtensions
{
  public static bool? TryGetBooleanProperty(this ItemEntity item, string key)
  {
    return item.Properties.TryGetValue(key, out string? value) ? bool.Parse(value) : null;
  }
  public static int? TryGetInt32Property(this ItemEntity item, string key)
  {
    return item.Properties.TryGetValue(key, out string? value) ? int.Parse(value) : null;
  }
  public static double? TryGetDoubleProperty(this ItemEntity item, string key)
  {
    return item.Properties.TryGetValue(key, out string? value) ? double.Parse(value) : null;
  }
  public static string? TryGetStringProperty(this ItemEntity item, string key)
  {
    return item.Properties.TryGetValue(key, out string? value) ? value : null;
  }
}
