import type { Ability } from "@/types/abilities";
import type { Move } from "@/types/moves";

export function formatAbility(ability: Ability): string {
  return ability.displayName ? `${ability.displayName} (${ability.uniqueName})` : ability.uniqueName;
}

export function formatMove(move: Move): string {
  return move.displayName ? `${move.displayName} (${move.uniqueName})` : move.uniqueName;
}
