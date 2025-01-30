using FluentValidation;
using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Validators;

internal class CreateOrReplaceMoveValidator : AbstractValidator<CreateOrReplaceMovePayload>
{
  public CreateOrReplaceMoveValidator()
  {
    RuleFor(x => x.Type).IsInEnum();
    RuleFor(x => x.Category).IsInEnum();

    RuleFor(x => x.UniqueName).UniqueName();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => x.Accuracy.HasValue, () => RuleFor(x => x.Accuracy!.Value).Accuracy());
    When(x => x.Power.HasValue, () => RuleFor(x => x.Power!.Value).Power());
    RuleFor(x => x.PowerPoints).PowerPoints();

    When(x => x.InflictedStatus != null, () => RuleFor(x => x.InflictedStatus!).SetValidator(new InflictedStatusValidator()));
    RuleForEach(x => x.StatisticChanges).SetValidator(new StatisticChangeValidator());
    RuleForEach(x => x.VolatileConditions).VolatileCondition();

    When(x => !string.IsNullOrWhiteSpace(x.Link), () => RuleFor(x => x.Link!).Url());
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).Notes());
  }
}
