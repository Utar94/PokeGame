import type { Aggregate } from "./aggregate";
import type { SearchPayload, SortOption } from "./search";

export type Ability = Aggregate & {
  uniqueName: string;
  displayName?: string;
  description?: string;
  reference?: string;
  notes?: string;
};

export type AbilitySort = "DisplayName" | "UniqueName" | "UpdatedOn";

export type AbilitySortOption = SortOption & {
  field: AbilitySort;
};

export type CreateAbilityPayload = {
  uniqueName: string;
  displayName?: string;
  description?: string;
  reference?: string;
  notes?: string;
};

export type ReplaceAbilityPayload = {
  uniqueName: string;
  displayName?: string;
  description?: string;
  reference?: string;
  notes?: string;
};

export type SearchAbilitiesPayload = SearchPayload & {
  sort: AbilitySortOption[];
};
