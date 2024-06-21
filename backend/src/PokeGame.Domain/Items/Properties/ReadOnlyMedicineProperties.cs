using FluentValidation;
using PokeGame.Contracts.Items.Properties;
using PokeGame.Domain.Items.Validators;

namespace PokeGame.Domain.Items.Properties;

public record ReadOnlyMedicineProperties : ItemProperties, IMedicineProperties
{
  public int? HitPointHealing { get; }
  public bool IsHitPointPercentage { get; }
  public bool DoesReviveFainted { get; }

  public string? RemoveStatusCondition { get; }
  public bool RemoveAllStatusConditions { get; }

  public int? RestorePowerPoints { get; }
  public bool IsPowerPointPercentage { get; }
  public bool RestoreAllMoves { get; }

  public int? FriendshipPenalty { get; }

  public ReadOnlyMedicineProperties()
  {
  }

  public ReadOnlyMedicineProperties(IMedicineProperties medicine) : this(medicine.HitPointHealing, medicine.IsHitPointPercentage, medicine.DoesReviveFainted,
    medicine.RemoveStatusCondition, medicine.RemoveAllStatusConditions, medicine.RestorePowerPoints, medicine.IsPowerPointPercentage, medicine.RestoreAllMoves,
    medicine.FriendshipPenalty)
  {
  }

  [JsonConstructor]
  public ReadOnlyMedicineProperties(int? hitPointHealing = null, bool isHitPointPercentage = false, bool doesReviveFainted = false,
    string? removeStatusCondition = null, bool removeAllStatusConditions = false, int? restorePowerPoints = null, bool isPowerPointPercentage = false,
    bool restoreAllMoves = false, int? friendshipPenalty = null)
  {
    HitPointHealing = hitPointHealing;
    IsHitPointPercentage = isHitPointPercentage;
    DoesReviveFainted = doesReviveFainted;

    RemoveStatusCondition = string.IsNullOrWhiteSpace(removeStatusCondition) ? null : new StatusCondition(removeStatusCondition).Value;
    RemoveAllStatusConditions = removeAllStatusConditions;

    RestorePowerPoints = restorePowerPoints;
    IsPowerPointPercentage = isPowerPointPercentage;
    RestoreAllMoves = restoreAllMoves;

    FriendshipPenalty = friendshipPenalty;

    new MedicinePropertiesValidator().ValidateAndThrow(this);
  }
}
