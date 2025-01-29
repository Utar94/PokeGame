using FluentValidation;

namespace PokeGame.Domain.Moves;

public class InflictedStatusValidator : AbstractValidator<IInflictedStatus>
{
  public InflictedStatusValidator()
  {
    RuleFor(x => x.Condition).IsInEnum();
    RuleFor(x => x.Chance).InclusiveBetween(1, 100);
  }
}
