using FluentValidation;

namespace PokeGame.Core.Species
{
  internal class SpeciesValidator : AbstractValidator<Species>
  {
    public SpeciesValidator()
    {
      RuleFor(x => x.Number)
        .InclusiveBetween(1, 999);

      RuleFor(x => x.SecondaryType)
        .NotEqual(x => x.PrimaryType);

      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Category)
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
