namespace PokeGame.Domain;

internal static class GlobalizationExtensions
{
  public static RegionInfo? GetRegion(this CultureInfo culture)
  {
    try
    {
      return new RegionInfo(culture.LCID);
    }
    catch (Exception)
    {
      return null;
    }
  }
}
