using FluentValidation;

namespace PokeGame.Domain.Moves.Validators;

public class AccuracyValidator : AbstractValidator<int>
{
  public AccuracyValidator()
  {
    RuleFor(x => x).GreaterThan(0).LessThanOrEqualTo(100);
  }
}
