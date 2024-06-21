import type { Ability } from "@/types/abilities";
import type { Item } from "@/types/items";
import type { Move } from "@/types/moves";
import type { Region } from "@/types/regions";

export function formatAbility(ability: Ability): string {
  return ability.displayName ? `${ability.displayName} (${ability.uniqueName})` : ability.uniqueName;
}

export function formatItem(item: Item): string {
  return item.displayName ? `${item.displayName} (${item.uniqueName})` : item.uniqueName;
}

export function formatMove(move: Move): string {
  return move.displayName ? `${move.displayName} (${move.uniqueName})` : move.uniqueName;
}

export function formatRegion(region: Region): string {
  return region.displayName ? `${region.displayName} (${region.uniqueName})` : region.uniqueName;
}
