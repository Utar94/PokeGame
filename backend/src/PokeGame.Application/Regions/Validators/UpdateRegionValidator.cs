using FluentValidation;
using PokeGame.Contracts.Regions;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Regions.Validators;

internal class UpdateRegionValidator : AbstractValidator<UpdateRegionPayload>
{
  public UpdateRegionValidator()
  {
    When(x => !string.IsNullOrWhiteSpace(x.Name), () => RuleFor(x => x.Name!).Name());
    When(x => !string.IsNullOrWhiteSpace(x.Description?.Value), () => RuleFor(x => x.Description!.Value!).Description());

    When(x => !string.IsNullOrWhiteSpace(x.Link?.Value), () => RuleFor(x => x.Link!.Value!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes?.Value), () => RuleFor(x => x.Notes!.Value!).Notes());
  }
}
