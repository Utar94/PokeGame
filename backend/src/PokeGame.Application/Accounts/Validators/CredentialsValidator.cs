using FluentValidation;
using Logitar.Identity.Domain.Users;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Validators;

internal class CredentialsValidator : AbstractValidator<Credentials>
{
  public CredentialsValidator()
  {
    RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(EmailUnit.MaximumLength).EmailAddress();
    When(x => x.Password != null, () => RuleFor(x => x.Password).NotEmpty());
  }
}
