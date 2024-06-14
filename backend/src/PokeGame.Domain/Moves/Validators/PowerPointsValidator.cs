using FluentValidation;

namespace PokeGame.Domain.Moves.Validators;

public class PowerPointsValidator : AbstractValidator<int>
{
  public PowerPointsValidator()
  {
    RuleFor(x => x).GreaterThan(0).LessThanOrEqualTo(40);
  }
}
