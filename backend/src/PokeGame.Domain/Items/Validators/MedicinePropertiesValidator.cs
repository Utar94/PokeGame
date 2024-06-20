using FluentValidation;
using PokeGame.Contracts.Items.Properties;

namespace PokeGame.Domain.Items.Validators;

public class MedicinePropertiesValidator : AbstractValidator<IMedicineProperties>
{
  public MedicinePropertiesValidator()
  {
    RuleFor(x => x.HitPoints).GreaterThan(0);
    When(x => x.IsHitPointPercentage, () => RuleFor(x => x.HitPoints).NotNull().LessThanOrEqualTo(100));
    When(x => x.IsReviveOrRemoveFainted, () => RuleFor(x => x.HitPoints).NotNull());

    When(x => !string.IsNullOrWhiteSpace(x.RemoveStatusCondition), () => RuleFor(x => x.RemoveStatusCondition).NotEmpty().MaximumLength(StatusCondition.MaximumLength));
    When(x => x.RemoveAllStatusConditions, () => RuleFor(x => x.RemoveStatusCondition).Empty());

    RuleFor(x => x.PowerPoints).GreaterThan(0);
    When(x => x.IsPowerPointPercentage, () => RuleFor(x => x.PowerPoints).NotNull().LessThanOrEqualTo(100));
    When(x => x.RestoreAllMovePowerPoints, () => RuleFor(x => x.PowerPoints).NotNull());

    RuleFor(x => x.FriendshipPenalty).GreaterThan(0).LessThanOrEqualTo(255);
  }
}
