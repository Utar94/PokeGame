using FluentValidation;
using PokeGame.Contracts.Moves;
using PokeGame.Domain;

namespace PokeGame.Application.Moves.Validators;

internal class InflictedStatusConditionValidator : AbstractValidator<InflictedStatusCondition>
{
  public InflictedStatusConditionValidator()
  {
    RuleFor(x => x.StatusCondition).NotEmpty().MaximumLength(StatusCondition.MaximumLength);
    RuleFor(x => x.Chance).GreaterThan(0).LessThanOrEqualTo(100);
  }
}
