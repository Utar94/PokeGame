using FluentValidation;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain;

namespace PokeGame.Application.Regions.Validators;

internal class CreateOrReplaceRegionValidator : AbstractValidator<CreateOrReplaceRegionPayload>
{
  public CreateOrReplaceRegionValidator()
  {
    RuleFor(x => x.UniqueName).UniqueName();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => !string.IsNullOrWhiteSpace(x.Link), () => RuleFor(x => x.Link!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).Notes());
  }
}
