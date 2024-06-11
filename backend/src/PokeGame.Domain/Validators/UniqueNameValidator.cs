using FluentValidation;
using Logitar.Identity.Contracts.Settings;

namespace PokeGame.Domain.Validators;

public class UniqueNameValidator : AbstractValidator<string>
{
  public UniqueNameValidator(IUniqueNameSettings uniqueNameSettings)
  {
    RuleFor(x => x).NotEmpty().MaximumLength(UniqueNameUnit.MaximumLength)
      .SetValidator(new AllowedCharactersValidator(uniqueNameSettings.AllowedCharacters));
  }
}
