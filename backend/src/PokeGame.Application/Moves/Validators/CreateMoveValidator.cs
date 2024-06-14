using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves.Validators;
using PokeGame.Domain.Validators;

namespace PokeGame.Application.Moves.Validators;

internal class CreateMoveValidator : AbstractValidator<CreateMovePayload>
{
  public CreateMoveValidator(IUniqueNameSettings uniqueNameSettings)
  {
    RuleFor(x => x.Type).IsInEnum();
    RuleFor(x => x.Category).IsInEnum();

    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).SetValidator(new DisplayNameValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).SetValidator(new DescriptionValidator()));

    When(x => x.Accuracy.HasValue, () => RuleFor(x => x.Accuracy!.Value).SetValidator(new AccuracyValidator()));
    When(x => x.Category == MoveCategory.Status, () => RuleFor(x => x.Power).Null().WithMessage($"'{{PropertyName}}' must be null when the move category is '{MoveCategory.Status}'."))
      .Otherwise(() => When(x => x.Power.HasValue, () => RuleFor(x => x.Power!.Value).SetValidator(new PowerValidator())));
    RuleFor(x => x.PowerPoints).SetValidator(new PowerPointsValidator());

    RuleForEach(x => x.StatusConditions).SetValidator(new InflictedStatusConditionValidator());
    RuleForEach(x => x.StatisticChanges).SetValidator(new StatisticChangeValidator());

    When(x => !string.IsNullOrWhiteSpace(x.Reference), () => RuleFor(x => x.Reference!).SetValidator(new UrlValidator()));
    When(x => !string.IsNullOrWhiteSpace(x.Notes), () => RuleFor(x => x.Notes!).SetValidator(new NotesValidator()));
  }
}
