import type { Aggregate } from "./aggregate";
import type { SearchPayload, SortOption } from "./search";

export type CreateRegionPayload = {
  uniqueName: string;
  displayName?: string;
  description?: string;
  reference?: string;
  notes?: string;
};

export type Region = Aggregate & {
  uniqueName: string;
  displayName?: string;
  description?: string;
  reference?: string;
  notes?: string;
};

export type RegionSort = "DisplayName" | "UniqueName" | "UpdatedOn";

export type RegionSortOption = SortOption & {
  field: RegionSort;
};

export type ReplaceRegionPayload = {
  uniqueName: string;
  displayName?: string;
  description?: string;
  reference?: string;
  notes?: string;
};

export type SearchRegionsPayload = SearchPayload & {
  sort: RegionSortOption[];
};
