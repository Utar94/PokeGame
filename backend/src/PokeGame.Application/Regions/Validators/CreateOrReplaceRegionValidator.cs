using FluentValidation;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Regions.Validators;

internal class CreateOrReplaceRegionValidator : AbstractValidator<CreateOrReplaceRegionPayload>
{
  public CreateOrReplaceRegionValidator()
  {
    RuleFor(x => x.Name).Name();
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => !string.IsNullOrWhiteSpace(x.Link), () => RuleFor(x => x.Link!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).Notes());
  }
}
