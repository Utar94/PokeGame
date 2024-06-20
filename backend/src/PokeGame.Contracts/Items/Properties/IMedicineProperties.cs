namespace PokeGame.Contracts.Items.Properties;

public interface IMedicineProperties
{
  int? HitPointHealing { get; }
  bool IsHitPointPercentage { get; }
  bool DoesReviveFainted { get; }

  string? RemoveStatusCondition { get; }
  bool RemoveAllStatusConditions { get; }

  int? RestorePowerPoints { get; }
  bool IsPowerPointPercentage { get; }
  bool RestoreAllMoves { get; }

  int? FriendshipPenalty { get; }
}
