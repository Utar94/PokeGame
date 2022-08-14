using System.Globalization;

namespace PokeGame.Web.Models.Api.Locale
{
  public class LocaleSummary
  {
    public LocaleSummary(CultureInfo culture)
    {
      ArgumentNullException.ThrowIfNull(culture);

      Code = culture.Name;
      DisplayName = culture.EnglishName;
    }

    public string Code { get; }
    public string DisplayName { get; }
  }
}
