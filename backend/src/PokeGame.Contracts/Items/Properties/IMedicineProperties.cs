namespace PokeGame.Contracts.Items.Properties;

public interface IMedicineProperties
{
  int? HitPoints { get; } // TODO(fpion): rename
  bool IsHitPointPercentage { get; } // TODO(fpion): rename
  bool IsReviveOrRemoveFainted { get; } // TODO(fpion): rename

  string? RemoveStatusCondition { get; } // TODO(fpion): rename
  bool RemoveAllStatusConditions { get; } // TODO(fpion): rename

  int? PowerPoints { get; } // TODO(fpion): rename
  bool IsPowerPointPercentage { get; } // TODO(fpion): rename
  bool RestoreAllMovePowerPoints { get; } // TODO(fpion): rename

  int? FriendshipPenalty { get; } // TODO(fpion): rename
}
