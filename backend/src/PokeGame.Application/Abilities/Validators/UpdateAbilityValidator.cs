using FluentValidation;
using PokeGame.Application.Abilities.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Abilities.Validators;

internal class UpdateAbilityValidator : AbstractValidator<UpdateAbilityPayload>
{
  public UpdateAbilityValidator()
  {
    When(x => !string.IsNullOrWhiteSpace(x.UniqueName), () => RuleFor(x => x.UniqueName!).UniqueName());
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName?.Value), () => RuleFor(x => x.DisplayName!.Value!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description?.Value), () => RuleFor(x => x.Description!.Value!).Description());

    When(x => !string.IsNullOrWhiteSpace(x.Link?.Value), () => RuleFor(x => x.Link!.Value!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes?.Value), () => RuleFor(x => x.Notes!.Value!).Notes());
  }
}
