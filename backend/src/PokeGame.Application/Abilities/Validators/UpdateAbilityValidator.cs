using FluentValidation;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Abilities.Validators;

internal class UpdateAbilityValidator : AbstractValidator<UpdateAbilityPayload>
{
  public UpdateAbilityValidator()
  {
    When(x => x.Kind?.Value != null, () => RuleFor(x => x.Kind!.Value).IsInEnum());

    When(x => !string.IsNullOrWhiteSpace(x.Name), () => RuleFor(x => x.Name!).Name());
    When(x => !string.IsNullOrWhiteSpace(x.Description?.Value), () => RuleFor(x => x.Description!.Value!).Description());

    When(x => !string.IsNullOrWhiteSpace(x.Link?.Value), () => RuleFor(x => x.Link!.Value!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes?.Value), () => RuleFor(x => x.Notes!.Value!).Notes());
  }
}
