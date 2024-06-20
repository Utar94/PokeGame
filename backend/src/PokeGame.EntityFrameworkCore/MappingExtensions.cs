using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore;

internal static class MappingExtensions
{
  public static double? TryGetDoubleProperty(this ItemEntity item, string key)
  {
    return item.Properties.TryGetValue(key, out string? value) ? double.Parse(value) : null;
  }
}
