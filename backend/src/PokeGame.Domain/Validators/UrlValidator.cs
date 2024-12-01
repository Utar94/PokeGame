using FluentValidation;
using FluentValidation.Validators;

namespace PokeGame.Domain.Validators;

public class UrlValidator<T> : IPropertyValidator<T, string>
{
  private static readonly HashSet<string> _allowedSchemes = ["http", "https"];

  public string Name { get; } = "UrlValidator";

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must be a valid absolute URL and start with 'http://' or 'https://'.";
  }

  public bool IsValid(ValidationContext<T> context, string value)
  {
    try
    {
      Uri uri = new(value, UriKind.Absolute);
      return _allowedSchemes.Contains(uri.Scheme.ToLowerInvariant());
    }
    catch (Exception)
    {
      return false;
    }
  }
}
