using FluentValidation;
using PokeGame.Domain.Validators;

namespace PokeGame.Domain;

public class Locale
{
  public const int MaximumLength = 16;

  public CultureInfo Culture { get; }
  public RegionInfo? Region { get; }

  public string Code { get; }
  public string LanguageCode => Culture.TwoLetterISOLanguageName;
  public string? RegionCode => Region?.TwoLetterISORegionName;

  public string DisplayName => Culture.DisplayName;
  public string EnglishName => Culture.EnglishName;
  public string NativeName => Culture.NativeName;

  public Locale(CultureInfo culture) : this(culture.Name)
  {
  }
  public Locale(string code)
  {
    Code = code.Trim();
    new Validator().ValidateAndThrow(this);

    Culture = CultureInfo.GetCultureInfo(code);
    Region = Culture.GetRegion();
  }

  public override bool Equals(object? obj) => obj is Locale locale && locale.Code == Code;
  public override int GetHashCode() => Code.GetHashCode();
  public override string ToString() => $"{DisplayName} ({Code})";

  private class Validator : AbstractValidator<Locale>
  {
    public Validator()
    {
      RuleFor(x => x.Code).Locale();
    }
  }
}
