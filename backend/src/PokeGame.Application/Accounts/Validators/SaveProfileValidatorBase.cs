using FluentValidation;
using PokeGame.Contracts.Accounts;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Accounts.Validators;

internal abstract class SaveProfileValidatorBase<T> : AbstractValidator<T> where T : SaveProfilePayload
{
  public SaveProfileValidatorBase()
  {
    RuleFor(x => x.MultiFactorAuthenticationMode).IsInEnum();

    RuleFor(x => x.FirstName).PersonName();
    When(x => !string.IsNullOrWhiteSpace(x.MiddleName), () => RuleFor(x => x.MiddleName!).PersonName());
    RuleFor(x => x.LastName).PersonName();

    When(x => x.Birthdate.HasValue, () => RuleFor(x => x.Birthdate!.Value).Birthdate());
    When(x => !string.IsNullOrWhiteSpace(x.Gender), () => RuleFor(x => x.Gender!).Gender());
    RuleFor(x => x.Locale).Locale();
    RuleFor(x => x.TimeZone).TimeZone();
  }
}
