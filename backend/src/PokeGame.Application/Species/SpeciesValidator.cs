using FluentValidation;

namespace PokeGame.Application.Species
{
  internal class SpeciesValidator : AbstractValidator<Domain.Species.Species>
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

      RuleFor(x => x.GenderRatio)
        .InclusiveBetween(0.0, 100.0);

      RuleFor(x => x.Height)
        .GreaterThan(0.0);

      RuleFor(x => x.Weight)
        .GreaterThan(0.0);

      RuleFor(x => x.BaseExperienceYield)
        .InclusiveBetween(1, 999);

      RuleFor(x => x.BaseFriendship)
        .LessThanOrEqualTo((byte)140)
        .Must(x => x % 5 == 0);

      RuleFor(x => x.CatchRate)
        .GreaterThan((byte)0);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);

      RuleForEach(x => x.BaseStatistics.Values)
        .GreaterThan((byte)0);

      RuleFor(x => x.EvYield)
        .Must(x => x.Values.Sum(y => y) <= 3);
    }
  }
}
