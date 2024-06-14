using FluentValidation;

namespace PokeGame.Domain.Moves.Validators;

public class PowerValidator : AbstractValidator<int>
{
  public PowerValidator()
  {
    RuleFor(x => x).GreaterThan(0).LessThanOrEqualTo(250);
  }
}
