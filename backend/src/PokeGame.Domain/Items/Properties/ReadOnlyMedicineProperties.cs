using PokeGame.Contracts.Items.Properties;

namespace PokeGame.Domain.Items.Properties;

public record ReadOnlyMedicineProperties : ItemProperties, IMedicineProperties
{
  public int? HitPoints { get; }
  public bool IsHitPointPercentage { get; }
  public bool IsReviveOrRemoveFainted { get; }

  public string? RemoveStatusCondition { get; }
  public bool RemoveAllStatusConditions { get; }

  public int? PowerPoints { get; }
  public bool IsPowerPointPercentage { get; }
  public bool RestoreAllMovePowerPoints { get; }

  public int? FriendshipPenalty { get; }

  public ReadOnlyMedicineProperties()
  {
  }

  public ReadOnlyMedicineProperties(IMedicineProperties medicine) : this(medicine.HitPoints, medicine.IsHitPointPercentage, medicine.IsReviveOrRemoveFainted,
    medicine.RemoveStatusCondition, medicine.RemoveAllStatusConditions, medicine.PowerPoints, medicine.IsPowerPointPercentage, medicine.RestoreAllMovePowerPoints,
    medicine.FriendshipPenalty) // TODO(fpion): rearrange
  {
  }

  public ReadOnlyMedicineProperties(int? hitPoints = null, bool isHitPointPercentage = false, bool isReviveOrRemoveFainted = false,
    string? removeStatusCondition = null, bool removeAllStatusConditions = false, int? powerPoints = null, bool isPowerPointPercentage = false,
    bool restoreAllMovePowerPoints = false, int? friendshipPenalty = null) // TODO(fpion): rename
  {
    HitPoints = hitPoints;
    IsHitPointPercentage = isHitPointPercentage;
    IsReviveOrRemoveFainted = isReviveOrRemoveFainted;

    RemoveStatusCondition = removeStatusCondition;
    RemoveAllStatusConditions = removeAllStatusConditions;

    PowerPoints = powerPoints;
    IsPowerPointPercentage = isPowerPointPercentage;
    RestoreAllMovePowerPoints = restoreAllMovePowerPoints;

    FriendshipPenalty = friendshipPenalty;
  }
}
