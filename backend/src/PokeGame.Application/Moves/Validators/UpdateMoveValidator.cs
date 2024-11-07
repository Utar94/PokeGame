using FluentValidation;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Moves.Validators;

internal class UpdateMoveValidator : AbstractValidator<UpdateMovePayload>
{
  public UpdateMoveValidator()
  {
    When(x => !string.IsNullOrWhiteSpace(x.UniqueName), () => RuleFor(x => x.UniqueName!).UniqueName());
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName?.Value), () => RuleFor(x => x.DisplayName!.Value!).Description());
    When(x => !string.IsNullOrWhiteSpace(x.Description?.Value), () => RuleFor(x => x.Description!.Value!).Description());

    When(x => x.Accuracy?.Value != null, () => RuleFor(x => x.Accuracy!.Value).GreaterThan(0).LessThanOrEqualTo(100));
    When(x => x.Power?.Value != null, () => RuleFor(x => x.Power!.Value).GreaterThan(0).LessThanOrEqualTo(Move.PowerMaximumValue));
    RuleFor(x => x.PowerPoints).GreaterThan(0).LessThanOrEqualTo(Move.PowerPointsMaximumValue);

    RuleForEach(x => x.StatisticChanges).SetValidator(new StatisticChangeValidator(allowZero: true));
    When(x => x.Status?.Value != null, () => RuleFor(x => x.Status!.Value!).SetValidator(new InflictedConditionValidator()));
    RuleForEach(x => x.VolatileConditions).SetValidator(new VolatileConditionUpdateValidator());

    When(x => !string.IsNullOrWhiteSpace(x.Link?.Value), () => RuleFor(x => x.Link!.Value!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes?.Value), () => RuleFor(x => x.Notes!.Value!).Notes());
  }
}
