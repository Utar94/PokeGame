import { urlUtils } from "logitar-js";

import type { CreateRegionPayload, Region, ReplaceRegionPayload, SearchRegionsPayload } from "@/types/regions";
import type { SearchResults } from "@/types/search";
import { _delete, get, post, put } from ".";

const path: string = "/regions";
function createUrlBuilder(id?: string): urlUtils.IUrlBuilder {
  return id ? new urlUtils.UrlBuilder({ path: `${path}/{id}` }).setParameter("id", id) : new urlUtils.UrlBuilder({ path });
}

export async function createRegion(payload: CreateRegionPayload): Promise<Region> {
  const url: string = createUrlBuilder().buildRelative();
  return (await post<CreateRegionPayload, Region>(url, payload)).data;
}

export async function deleteRegion(id: string): Promise<Region> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await _delete<Region>(url)).data;
}

export async function readRegion(id: string): Promise<Region> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await get<Region>(url)).data;
}

export async function replaceRegion(id: string, payload: ReplaceRegionPayload, version?: number): Promise<Region> {
  const url: string = createUrlBuilder(id)
    .setQuery("version", version?.toString() ?? "")
    .buildRelative();
  return (await put<ReplaceRegionPayload, Region>(url, payload)).data;
}

export async function searchRegions(payload: SearchRegionsPayload): Promise<SearchResults<Region>> {
  const url: string = createUrlBuilder()
    .setQuery("ids", payload.ids)
    .setQuery(
      "search_terms",
      payload.search.terms.map(({ value }) => value),
    )
    .setQuery("search_operator", payload.search.operator)
    .setQuery(
      "sort",
      payload.sort.map(({ field, isDescending }) => (isDescending ? `DESC.${field}` : field)),
    )
    .setQuery("skip", payload.skip.toString())
    .setQuery("limit", payload.limit.toString())
    .buildRelative();
  return (await get<SearchResults<Region>>(url)).data;
}
