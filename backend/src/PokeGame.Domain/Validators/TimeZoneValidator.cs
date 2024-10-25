using FluentValidation;
using FluentValidation.Validators;
using NodaTime;

namespace PokeGame.Domain.Validators;

internal class TimeZoneValidator<T> : IPropertyValidator<T, string>
{
  public string Name { get; } = "TimeZoneValidator";

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' did not resolve to a tz entry.";
  }

  public bool IsValid(ValidationContext<T> context, string value)
  {
    return DateTimeZoneProviders.Tzdb.GetZoneOrNull(value) != null;
  }
}
