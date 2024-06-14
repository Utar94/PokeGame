import type { Aggregate } from "./aggregate";
import type { PokemonStatistic, PokemonType } from "./pokemon";
import type { SearchPayload, SortOption } from "./search";

export type CreateMovePayload = {
  type: PokemonType;
  category: MoveCategory;
  uniqueName: string;
  displayName?: string;
  description?: string;
  accuracy?: number;
  power?: number;
  powerPoints: number;
  statisticChanges: StatisticChange[];
  statusConditions: InflictedStatusCondition[];
  reference?: string;
  notes?: string;
};

export type InflictedStatusCondition = {
  statusCondition: string;
  chance: number;
};

export type Move = Aggregate & {
  type: PokemonType;
  category: MoveCategory;
  uniqueName: string;
  displayName?: string;
  description?: string;
  accuracy?: number;
  power?: number;
  powerPoints: number;
  statisticChanges: StatisticChange[];
  statusConditions: InflictedStatusCondition[];
  reference?: string;
  notes?: string;
};

export type MoveCategory = "Status" | "Physical" | "Special";

export type MoveSort = "Accuracy" | "DisplayName" | "Power" | "PowerPoints" | "UniqueName" | "UpdatedOn";

export type MoveSortOption = SortOption & {
  field: MoveSort;
};

export type ReplaceMovePayload = {
  uniqueName: string;
  displayName?: string;
  description?: string;
  accuracy?: number;
  power?: number;
  powerPoints: number;
  statisticChanges: StatisticChange[];
  statusConditions: InflictedStatusCondition[];
  reference?: string;
  notes?: string;
};

export type SearchMovesPayload = SearchPayload & {
  type?: PokemonType;
  category?: MoveCategory;
  sort: MoveSortOption[];
};

export type StatisticChange = {
  statistic: PokemonStatistic;
  stages: number;
};
