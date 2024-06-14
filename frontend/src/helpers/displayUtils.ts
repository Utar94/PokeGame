import type { Ability } from "@/types/abilities";

export function formatAbility(ability: Ability): string {
  return ability.displayName ? `${ability.displayName} (${ability.uniqueName})` : ability.uniqueName;
}
