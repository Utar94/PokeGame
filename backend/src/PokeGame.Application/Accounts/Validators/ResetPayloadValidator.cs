using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using PokeGame.Contracts.Accounts;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Accounts.Validators;

internal class ResetPayloadValidator : AbstractValidator<ResetPayload>
{
  public ResetPayloadValidator(IPasswordSettings passwordSettings)
  {
    RuleFor(x => x.Token).NotEmpty();
    RuleFor(x => x.Password).Password(passwordSettings);
  }
}
