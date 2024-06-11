using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Abilities.Validators;

internal class ReplaceAbilityValidator : AbstractValidator<ReplaceAbilityPayload>
{
  public ReplaceAbilityValidator(IUniqueNameSettings uniqueNameSettings)
  {
    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).SetValidator(new DisplayNameValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).SetValidator(new DescriptionValidator()));

    When(x => !string.IsNullOrWhiteSpace(x.Reference), () => RuleFor(x => x.Reference!).SetValidator(new ReferenceValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).SetValidator(new NotesValidator()));
  }
}
