namespace PokeGame.Contracts.Items.Properties;

public record MedicineProperties : IMedicineProperties
{
  public int? HitPointHealing { get; set; }
  public bool IsHitPointPercentage { get; set; }
  public bool DoesReviveFainted { get; set; }

  public string? RemoveStatusCondition { get; set; }
  public bool RemoveAllStatusConditions { get; set; }

  public int? RestorePowerPoints { get; set; }
  public bool IsPowerPointPercentage { get; set; }
  public bool RestoreAllMoves { get; set; }

  public int? FriendshipPenalty { get; set; }

  public MedicineProperties()
  {
  }

  public MedicineProperties(IMedicineProperties medicine) : this()
  {
    HitPointHealing = medicine.HitPointHealing;
    IsHitPointPercentage = medicine.IsHitPointPercentage;
    DoesReviveFainted = medicine.DoesReviveFainted;

    RemoveStatusCondition = medicine.RemoveStatusCondition;
    RemoveAllStatusConditions = medicine.RemoveAllStatusConditions;

    RestorePowerPoints = medicine.RestorePowerPoints;
    IsPowerPointPercentage = medicine.IsPowerPointPercentage;
    RestoreAllMoves = medicine.RestoreAllMoves;

    FriendshipPenalty = medicine.FriendshipPenalty;
  }
}
