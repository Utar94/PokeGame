using FluentValidation;
using FluentValidation.Validators;

namespace PokeGame.Domain.Validators;

internal class PastValidator<T> : IPropertyValidator<T, DateTime>
{
  public string Name { get; } = "PastValidator";
  public DateTime Now { get; }

  public PastValidator(DateTime? now = null)
  {
    Now = now ?? DateTime.Now;
  }

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must be a date and time set in the past.";
  }

  public bool IsValid(ValidationContext<T> context, DateTime value)
  {
    return value < Now;
  }
}
