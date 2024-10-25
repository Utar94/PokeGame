using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using PokeGame.Contracts.Accounts;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Accounts.Validators;

internal class CompleteProfileValidator : SaveProfileValidatorBase<CompleteProfilePayload>
{
  public CompleteProfileValidator(IPasswordSettings passwordSettings) : base()
  {
    RuleFor(x => x.Token).NotEmpty();

    When(x => x.Password != null, () => RuleFor(x => x.Password!).Password(passwordSettings));
  }
}
