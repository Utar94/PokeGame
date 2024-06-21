import { urlUtils } from "logitar-js";

import type { CreateItemPayload, Item, ReplaceItemPayload, SearchItemsPayload } from "@/types/items";
import type { SearchResults } from "@/types/search";
import { _delete, get, post, put } from ".";

const path: string = "/items";
function createUrlBuilder(id?: string): urlUtils.IUrlBuilder {
  return id ? new urlUtils.UrlBuilder({ path: `${path}/{id}` }).setParameter("id", id) : new urlUtils.UrlBuilder({ path });
}

export async function createItem(payload: CreateItemPayload): Promise<Item> {
  const url: string = createUrlBuilder().buildRelative();
  return (await post<CreateItemPayload, Item>(url, payload)).data;
}

export async function deleteItem(id: string): Promise<Item> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await _delete<Item>(url)).data;
}

export async function readItem(id: string): Promise<Item> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await get<Item>(url)).data;
}

export async function replaceItem(id: string, payload: ReplaceItemPayload, version?: number): Promise<Item> {
  const url: string = createUrlBuilder(id)
    .setQuery("version", version?.toString() ?? "")
    .buildRelative();
  return (await put<ReplaceItemPayload, Item>(url, payload)).data;
}

export async function searchItems(payload: SearchItemsPayload): Promise<SearchResults<Item>> {
  const url: string = createUrlBuilder()
    .setQuery("category", payload.category ?? "")
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
  return (await get<SearchResults<Item>>(url)).data;
}
