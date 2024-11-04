using FluentValidation;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Moves.Validators;

internal class CreateOrReplaceMoveValidator : AbstractValidator<CreateOrReplaceMovePayload>
{
  public CreateOrReplaceMoveValidator()
  {
    RuleFor(x => x.Name).Name();
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => x.Accuracy != null, () => RuleFor(x => x.Accuracy).GreaterThan(0).LessThanOrEqualTo(100));
    When(x => x.Power != null, () => RuleFor(x => x.Power).GreaterThan(0).LessThan(Move.PowerMaximumValue));
    RuleFor(x => x.PowerPoints).GreaterThan(0).LessThan(Move.PowerPointsMaximumValue);

    RuleForEach(x => x.StatisticChanges).SetValidator(new StatisticChangeValidator());
    When(x => x.Status != null, () => RuleFor(x => x.Status!).SetValidator(new InflictedConditionValidator()));
    RuleForEach(x => x.VolatileConditions).MaximumLength(VolatileCondition.MaximumLength);

    When(x => !string.IsNullOrWhiteSpace(x.Link), () => RuleFor(x => x.Link!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).Notes());
  }
}
