using FluentValidation;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions
{
  internal class RegionValidator : AbstractValidator<Region>
  {
    public RegionValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
