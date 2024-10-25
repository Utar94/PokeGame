using FluentValidation;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Application.Accounts.Validators;

internal class OneTimePasswordValidator : AbstractValidator<OneTimePasswordPayload>
{
  public OneTimePasswordValidator()
  {
    RuleFor(x => x.Id).NotEmpty();
    RuleFor(x => x.Code).NotEmpty();
  }
}
