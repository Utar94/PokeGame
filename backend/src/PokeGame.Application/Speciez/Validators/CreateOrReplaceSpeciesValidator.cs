using FluentValidation;
using PokeGame.Application.Speciez.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Speciez.Validators;

internal class CreateOrReplaceSpeciesValidator : AbstractValidator<CreateOrReplaceSpeciesPayload>
{
  public CreateOrReplaceSpeciesValidator()
  {
    RuleFor(x => x.Number).GreaterThan(0);
    RuleFor(x => x.Category).IsInEnum();

    RuleFor(x => x.UniqueName).UniqueName();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());

    RuleFor(x => x.GrowthRate).IsInEnum();
    RuleFor(x => x.BaseFriendship).Friendship();
    RuleFor(x => x.CatchRate).CatchRate();

    RuleForEach(x => x.RegionalNumbers).SetValidator(new RegionalNumberValidator());

    When(x => !string.IsNullOrWhiteSpace(x.Link), () => RuleFor(x => x.Link!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).Notes());
  }
}
