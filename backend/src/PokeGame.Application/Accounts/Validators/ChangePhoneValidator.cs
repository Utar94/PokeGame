using FluentValidation;
using PokeGame.Contracts.Accounts;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Accounts.Validators;

internal class ChangePhoneValidator : AbstractValidator<ChangePhonePayload>
{
  public ChangePhoneValidator()
  {
    RuleFor(x => x.Locale).Locale();

    When(x => x.Phone != null, () => RuleFor(x => x.Phone!).SetValidator(new AccountPhoneValidator()));
    When(x => x.OneTimePassword != null, () => RuleFor(x => x.OneTimePassword!).SetValidator(new OneTimePasswordValidator()));

    RuleFor(x => x).Must(BeAValidPayload).WithErrorCode(nameof(ChangePhoneValidator))
      .WithMessage(x => $"Exactly one of the following must be specified: {nameof(x.Phone)}, {nameof(x.OneTimePassword)}.");
  }

  private static bool BeAValidPayload(ChangePhonePayload payload)
  {
    int count = 0;
    if (payload.Phone != null)
    {
      count++;
    }
    if (payload.OneTimePassword != null)
    {
      count++;
    }
    return count == 1;
  }
}
