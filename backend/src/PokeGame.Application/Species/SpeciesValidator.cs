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

      RuleFor(x => x.EggCycles)
        .GreaterThan((byte)0);

      RuleFor(x => x.Picture)
        .Must(ValidationRules.BeAValidUrl);

      When(x => x.Picture == null, () =>
      {
        RuleFor(x => x.PictureShiny)
          .Null();
      }).Otherwise(() =>
      {
        RuleFor(x => x.PictureShiny)
          .Must(ValidationRules.BeAValidUrl);
      });

      When(x => x.GenderRatio == null || x.GenderRatio == 100.0, () =>
      {
        RuleFor(x => x.PictureFemale)
          .Null();
        RuleFor(x => x.PictureShinyFemale)
          .Null();
      }).Otherwise(() =>
      {
        When(x => x.Picture == null, () =>
        {
          RuleFor(x => x.PictureFemale)
            .Null();
        }).Otherwise(() =>
        {
          RuleFor(x => x.PictureFemale)
            .Must(ValidationRules.BeAValidUrl);
        });

        When(x => x.PictureFemale == null || x.PictureShiny == null, () =>
        {
          RuleFor(x => x.PictureShinyFemale)
            .Null();
        }).Otherwise(() =>
        {
          RuleFor(x => x.PictureShinyFemale)
            .Must(ValidationRules.BeAValidUrl);
        });
      });

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);

      RuleForEach(x => x.BaseStatistics.Values)
        .GreaterThan((byte)0);

      RuleFor(x => x.EvYield)
        .Must(x => x.Values.Sum(y => y) <= 3);

      RuleForEach(x => x.Evolutions.Values)
        .SetValidator(new EvolutionValidator());

      RuleForEach(x => x.RegionalNumbers.Values)
        .InclusiveBetween(1, 999);
    }
  }
}
