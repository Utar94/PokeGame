using FluentValidation;
using PokeGame.Contracts.Moves;

namespace PokeGame.Domain.Moves;

public class InflictedConditionValidator : AbstractValidator<IInflictedCondition>
{
  public InflictedConditionValidator()
  {
    RuleFor(x => x.Condition).IsInEnum();
    RuleFor(x => x.Chance).GreaterThan(0).LessThanOrEqualTo(100);
  }
}
