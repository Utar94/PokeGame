using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using PokeGame.Contracts.Accounts;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Accounts.Validators;

internal class ChangePasswordValidator : AbstractValidator<ChangeAccountPasswordPayload>
{
  public ChangePasswordValidator(IPasswordSettings passwordSettings)
  {
    RuleFor(x => x.Current).NotEmpty();
    RuleFor(x => x.New).Password(passwordSettings);
  }
}
