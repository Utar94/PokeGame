import { urlUtils } from "logitar-js";

import type { CreateMovePayload, Move, ReplaceMovePayload, SearchMovesPayload } from "@/types/moves";
import type { SearchResults } from "@/types/search";
import { _delete, get, post, put } from ".";

const path: string = "/moves";
function createUrlBuilder(id?: string): urlUtils.IUrlBuilder {
  return id ? new urlUtils.UrlBuilder({ path: `${path}/{id}` }).setParameter("id", id) : new urlUtils.UrlBuilder({ path });
}

export async function createMove(payload: CreateMovePayload): Promise<Move> {
  const url: string = createUrlBuilder().buildRelative();
  return (await post<CreateMovePayload, Move>(url, payload)).data;
}

export async function deleteMove(id: string): Promise<Move> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await _delete<Move>(url)).data;
}

export async function readMove(id: string): Promise<Move> {
  const url: string = createUrlBuilder(id).buildRelative();
  return (await get<Move>(url)).data;
}

export async function replaceMove(id: string, payload: ReplaceMovePayload, version?: number): Promise<Move> {
  const url: string = createUrlBuilder(id)
    .setQuery("version", version?.toString() ?? "")
    .buildRelative();
  return (await put<ReplaceMovePayload, Move>(url, payload)).data;
}

export async function searchMoves(payload: SearchMovesPayload): Promise<SearchResults<Move>> {
  const url: string = createUrlBuilder()
    .setQuery("type", payload.type ?? "")
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
  return (await get<SearchResults<Move>>(url)).data;
}
