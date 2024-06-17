import { urlUtils } from "logitar-js";

import type { Ability, CreateAbilityPayload, ReplaceAbilityPayload, SearchAbilitiesPayload } from "@/types/abilities";
import type { SearchResults } from "@/types/search";
import { _delete, get, post, put } from ".";

const path: string = "/abilities";
function createUrlBuilder(id?: string): urlUtils.IUrlBuilder {
  return id ? new urlUtils.UrlBuilder({ path: `${path}/{id}` }).setParameter("id", id) : new urlUtils.UrlBuilder({ path });
}

export async function createAbility(payload: CreateAbilityPayload): Promise<Ability> {
  const url: string = createUrlBuilder().buildRelative();
  return (await post<CreateAbilityPayload, Ability>(url, payload)).data;
}

export async function deleteAbility(id: string): Promise<Ability> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await _delete<Ability>(url)).data;
}

export async function readAbility(id: string): Promise<Ability> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await get<Ability>(url)).data;
}

export async function replaceAbility(id: string, payload: ReplaceAbilityPayload, version?: number): Promise<Ability> {
  const url: string = createUrlBuilder(id)
    .setQuery("version", version?.toString() ?? "")
    .buildRelative();
  return (await put<ReplaceAbilityPayload, Ability>(url, payload)).data;
}

export async function searchAbilities(payload: SearchAbilitiesPayload): Promise<SearchResults<Ability>> {
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
  return (await get<SearchResults<Ability>>(url)).data;
}
