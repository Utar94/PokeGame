﻿using FluentValidation;
using Logitar.Portal.Contracts.Users;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Validators;

internal class AccountPhoneValidator : AbstractValidator<AccountPhone>
{
  public AccountPhoneValidator()
  {
    RuleFor(x => x.CountryCode).NotEmpty().Length(2);
    RuleFor(x => x.Number).NotEmpty().MaximumLength(20);

    RuleFor(x => x).Must(BeAValidPhone).WithErrorCode("PhoneValidator").WithMessage("'{PropertyName}' must be a valid phone.");
  }

  private static bool BeAValidPhone(AccountPhone input)
  {
    Phone phone = new(input.CountryCode, input.Number, extension: null, e164Formatted: string.Empty);
    return phone.IsValid();
  }
}
