using FluentValidation;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Abilities.Validators;

internal class CreateOrReplaceAbilityValidator : AbstractValidator<CreateOrReplaceAbilityPayload>
{
  public CreateOrReplaceAbilityValidator()
  {
    When(x => x.Kind.HasValue, () => RuleFor(x => x.Kind).IsInEnum());

    RuleFor(x => x.Name).Name();
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => !string.IsNullOrWhiteSpace(x.Link), () => RuleFor(x => x.Link!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).Notes());
  }
}
