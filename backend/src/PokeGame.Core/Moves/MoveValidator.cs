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

      When(x => x.Category == MoveCategory.Status, () => RuleFor(x => x.Power).Null());

      RuleFor(x => x.Accuracy)
        .LessThanOrEqualTo((byte)100)
        .Must(x => x == null || x % 5 == 0);

      RuleFor(x => x.Power)
        .Must(x => x == null || x % 5 == 0);

      RuleFor(x => x.PowerPoints)
        .InclusiveBetween((byte)5, (byte)40)
        .Must(x => x % 5 == 0);

      RuleFor(x => x.Reference)
        .Must(ValidationRules.BeAValidUrl);
    }
  }
}
