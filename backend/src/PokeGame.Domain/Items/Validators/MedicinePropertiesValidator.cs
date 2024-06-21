using FluentValidation;
using PokeGame.Contracts.Items.Properties;

namespace PokeGame.Domain.Items.Validators;

public class MedicinePropertiesValidator : AbstractValidator<IMedicineProperties>
{
  public MedicinePropertiesValidator()
  {
    RuleFor(x => x.HitPointHealing).GreaterThan(0);
    When(x => x.IsHitPointPercentage, () => RuleFor(x => x.HitPointHealing).NotNull().LessThanOrEqualTo(100));
    When(x => x.DoesReviveFainted, () => RuleFor(x => x.HitPointHealing).NotNull());

    When(x => !string.IsNullOrWhiteSpace(x.RemoveStatusCondition), () => RuleFor(x => x.RemoveStatusCondition).NotEmpty().MaximumLength(StatusCondition.MaximumLength));
    When(x => x.RemoveAllStatusConditions, () => RuleFor(x => x.RemoveStatusCondition).Empty());

    RuleFor(x => x.RestorePowerPoints).GreaterThan(0);
    When(x => x.IsPowerPointPercentage, () => RuleFor(x => x.RestorePowerPoints).NotNull().LessThanOrEqualTo(100));
    When(x => x.RestoreAllMoves, () => RuleFor(x => x.RestorePowerPoints).NotNull());

    RuleFor(x => x.FriendshipPenalty).GreaterThan(0).LessThanOrEqualTo(255);
  }
}
