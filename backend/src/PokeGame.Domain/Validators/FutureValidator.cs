using FluentValidation;
using FluentValidation.Validators;

namespace PokeGame.Domain.Validators;

internal class FutureValidator<T> : IPropertyValidator<T, DateTime>
{
  public string Name { get; } = "FutureValidator";
  public DateTime Now { get; }

  public FutureValidator(DateTime? now = null)
  {
    Now = now ?? DateTime.Now;
  }

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must be a date and time set in the future.";
  }

  public bool IsValid(ValidationContext<T> context, DateTime value)
  {
    return value > Now;
  }
}
