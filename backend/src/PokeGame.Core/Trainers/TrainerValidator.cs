using FluentValidation;

namespace PokeGame.Core.Trainers
{
  internal class TrainerValidator : AbstractValidator<Trainer>
  {
    public TrainerValidator()
    {
      RuleFor(x => x.Number)
        .InclusiveBetween(100000, 999999);

      RuleFor(x => x.Money)
        .GreaterThanOrEqualTo(0);

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
