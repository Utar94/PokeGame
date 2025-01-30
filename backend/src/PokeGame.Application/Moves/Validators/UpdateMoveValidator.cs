using FluentValidation;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Validators;

internal class UpdateMoveValidator : AbstractValidator<UpdateMovePayload>
{
  public UpdateMoveValidator()
  {
    When(x => !string.IsNullOrWhiteSpace(x.UniqueName), () => RuleFor(x => x.UniqueName!).UniqueName());
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName?.Value), () => RuleFor(x => x.DisplayName!.Value!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description?.Value), () => RuleFor(x => x.Description!.Value!).Description());

    When(x => x.Accuracy?.Value != null, () => RuleFor(x => x.Accuracy!.Value!.Value).Accuracy());
    When(x => x.Power?.Value != null, () => RuleFor(x => x.Power!.Value!.Value).Power());
    When(x => x.PowerPoints.HasValue, () => RuleFor(x => x.PowerPoints!.Value).PowerPoints());

    When(x => x.InflictedStatus?.Value != null, () => RuleFor(x => x.InflictedStatus!.Value!).SetValidator(new InflictedStatusValidator()));
    RuleForEach(x => x.StatisticChanges).SetValidator(new StatisticChangeValidator());
    RuleForEach(x => x.VolatileConditions).SetValidator(new VolatileConditionActionValidator());

    When(x => !string.IsNullOrWhiteSpace(x.Link?.Value), () => RuleFor(x => x.Link!.Value!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes?.Value), () => RuleFor(x => x.Notes!.Value!).Notes());
  }
}
