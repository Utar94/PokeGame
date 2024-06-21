import type { Aggregate } from "./aggregate";
import type { SearchPayload, SortOption } from "./search";

export type CreateItemPayload = {
  category: ItemCategory;
  price?: number;
  uniqueName: string;
  displayName?: string;
  description?: string;
  picture?: string;
  medicine?: MedicineProperties;
  pokeBall?: PokeBallProperties;
  reference?: string;
  notes?: string;
};

export type Item = Aggregate & {
  category: ItemCategory;
  price?: number;
  uniqueName: string;
  displayName?: string;
  description?: string;
  picture?: string;
  medicine?: MedicineProperties;
  pokeBall?: PokeBallProperties;
  reference?: string;
  notes?: string;
};

export type ItemCategory = "BattleItem" | "Berry" | "KeyItem" | "Medicine" | "OtherItem" | "PokeBall" | "TM" | "Treasure";

export type ItemSort = "DisplayName" | "Price" | "UniqueName" | "UpdatedOn";

export type ItemSortOption = SortOption & {
  field: ItemSort;
};

export type MedicineProperties = {
  hitPointHealing?: number;
  isHitPointPercentage: boolean;
  doesReviveFainted: boolean;
  removeStatusCondition?: string;
  removeAllStatusConditions: boolean;
  restorePowerPoints?: number;
  isPowerPointPercentage: boolean;
  restoreAllMoves: boolean;
  friendshipPenalty?: number;
};

export type PokeBallProperties = {
  catchRateModifier?: number;
};

export type ReplaceItemPayload = {
  price?: number;
  uniqueName: string;
  displayName?: string;
  description?: string;
  picture?: string;
  medicine?: MedicineProperties;
  pokeBall?: PokeBallProperties;
  reference?: string;
  notes?: string;
};

export type SearchItemsPayload = SearchPayload & {
  category?: ItemCategory;
  sort: ItemSortOption[];
};
