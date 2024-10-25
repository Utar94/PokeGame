using FluentValidation;
using FluentValidation.Validators;

namespace PokeGame.Domain.Validators;

internal class UrlValidator<T> : IPropertyValidator<T, string>
{
  private static readonly HashSet<string> _schemes = ["http", "https"];

  public string Name { get; } = "UrlValidator";

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return $"'{{PropertyName}}' must be a valid absolute URL and start with one of the following schemes: {string.Join(", ", _schemes)}.";
  }

  public bool IsValid(ValidationContext<T> context, string value)
  {
    try
    {
      Uri uri = new(value, UriKind.Absolute);
      return _schemes.Contains(uri.Scheme.ToLowerInvariant());
    }
    catch (Exception)
    {
      return false;
    }
  }
}
