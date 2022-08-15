using FluentValidation;

namespace PokeGame.Core.Moves
{
  internal class MoveValidator : AbstractValidator<Move>
  {
    public MoveValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(x => x.Description)
        .MaximumLength(1000);

      When(x => x.Category == Category.Status, () => RuleFor(x => x.Power).Null());

      RuleFor(x => x.Accuracy)
        .InclusiveBetween(0.0, 1.0)
        .Must(x => x == null || x % 5 == 0);

      RuleFor(x => x.Power)
        .InclusiveBetween(5, 250)
        .Must(x => x == null || x % 5 == 0);

      RuleFor(x => x.PowerPoints)
        .InclusiveBetween(5, 40)
        .Must(x => x % 5 == 0);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
