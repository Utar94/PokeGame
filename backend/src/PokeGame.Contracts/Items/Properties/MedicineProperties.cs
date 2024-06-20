namespace PokeGame.Contracts.Items.Properties;

public record MedicineProperties : IMedicineProperties
{
  public int? HitPoints { get; set; }
  public bool IsHitPointPercentage { get; set; }
  public bool IsReviveOrRemoveFainted { get; set; }

  public string? RemoveStatusCondition { get; set; }
  public bool RemoveAllStatusConditions { get; set; }

  public int? PowerPoints { get; set; }
  public bool IsPowerPointPercentage { get; set; }
  public bool RestoreAllMovePowerPoints { get; set; }

  public int? FriendshipPenalty { get; set; }

  public MedicineProperties()
  {
  }

  public MedicineProperties(IMedicineProperties medicine) : this()
  {
    HitPoints = medicine.HitPoints;
    IsHitPointPercentage = medicine.IsHitPointPercentage;
    IsReviveOrRemoveFainted = medicine.IsReviveOrRemoveFainted;

    RemoveStatusCondition = medicine.RemoveStatusCondition;
    RemoveAllStatusConditions = medicine.RemoveAllStatusConditions;

    PowerPoints = medicine.PowerPoints;
    IsPowerPointPercentage = medicine.IsPowerPointPercentage;
    RestoreAllMovePowerPoints = medicine.RestoreAllMovePowerPoints;

    FriendshipPenalty = medicine.FriendshipPenalty;
  }
}
