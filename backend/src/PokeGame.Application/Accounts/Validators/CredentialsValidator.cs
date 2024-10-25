using FluentValidation;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Validators;

internal class CredentialsValidator : AbstractValidator<Credentials>
{
  private const int EmailAddressMaximumLength = byte.MaxValue;

  public CredentialsValidator()
  {
    RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(EmailAddressMaximumLength).EmailAddress();
    When(x => x.Password != null, () => RuleFor(x => x.Password).NotEmpty());
  }
}
