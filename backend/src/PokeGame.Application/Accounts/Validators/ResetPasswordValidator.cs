﻿using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using PokeGame.Contracts.Accounts;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Accounts.Validators;

internal class ResetPasswordValidator : AbstractValidator<ResetPasswordPayload>
{
  public ResetPasswordValidator(IPasswordSettings passwordSettings)
  {
    RuleFor(x => x.Locale).Locale();

    When(x => x.Reset != null, () => RuleFor(x => x.Reset!).SetValidator(new ResetPayloadValidator(passwordSettings)));

    RuleFor(x => x).Must(BeAValidPayload).WithErrorCode(nameof(ResetPasswordValidator))
      .WithMessage(x => $"Exactly one of the following must be specified: {nameof(x.EmailAddress)}, {nameof(x.Reset)}.");
  }

  private static bool BeAValidPayload(ResetPasswordPayload payload)
  {
    int count = 0;
    if (payload.EmailAddress != null)
    {
      count++;
    }
    if (payload.Reset != null)
    {
      count++;
    }
    return count == 1;
  }
}
