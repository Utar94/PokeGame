using FluentValidation;
using PokeGame.Application.Speciez.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Speciez.Validators;

internal class UpdateSpeciesValidator : AbstractValidator<UpdateSpeciesPayload>
{
  public UpdateSpeciesValidator()
  {
    When(x => !string.IsNullOrWhiteSpace(x.UniqueName), () => RuleFor(x => x.UniqueName!).UniqueName());
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName?.Value), () => RuleFor(x => x.DisplayName!.Value!).DisplayName());

    When(x => x.GrowthRate.HasValue, () => RuleFor(x => x.GrowthRate!.Value).IsInEnum());
    When(x => x.BaseFriendship.HasValue, () => RuleFor(x => x.BaseFriendship!.Value).Friendship());
    When(x => x.CatchRate.HasValue, () => RuleFor(x => x.CatchRate!.Value).CatchRate());

    RuleForEach(x => x.RegionalNumbers).SetValidator(new RegionalNumberUpdateValidator());

    When(x => !string.IsNullOrWhiteSpace(x.Link?.Value), () => RuleFor(x => x.Link!.Value!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes?.Value), () => RuleFor(x => x.Notes!.Value!).Notes());
  }
}
