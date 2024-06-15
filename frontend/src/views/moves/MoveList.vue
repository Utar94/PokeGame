<script setup lang="ts">
import { TarButton, parsingUtils, type SelectOption } from "logitar-vue3-ui";
import { arrayUtils, objectUtils } from "logitar-js";
import { computed, inject, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { useRoute, useRouter } from "vue-router";

import AppPagination from "@/components/shared/AppPagination.vue";
import CountSelect from "@/components/shared/CountSelect.vue";
import CreateMove from "@/components/moves/CreateMove.vue";
import MoveCategorySelect from "@/components/moves/MoveCategorySelect.vue";
import PokemonTypeSelect from "@/components/pokemon/PokemonTypeSelect.vue";
import SearchInput from "@/components/shared/SearchInput.vue";
import SortSelect from "@/components/shared/SortSelect.vue";
import StatusBlock from "@/components/shared/StatusBlock.vue";
import type { Move, MoveCategory, MoveSort, SearchMovesPayload } from "@/types/moves";
import type { PokemonType } from "@/types/pokemon";
import { handleErrorKey } from "@/inject/App";
import { searchMoves } from "@/api/moves";

const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const { isEmpty } = objectUtils;
const { orderBy } = arrayUtils;
const { parseBoolean, parseNumber } = parsingUtils;
const { n, rt, t, tm } = useI18n();

const isLoading = ref<boolean>(false);
const moves = ref<Move[]>([]);
const timestamp = ref<number>(0);
const total = ref<number>(0);

const category = computed<MoveCategory>(() => (route.query.category?.toString() as MoveCategory) ?? "");
const count = computed<number>(() => parseNumber(route.query.count?.toString()) || 10);
const isDescending = computed<boolean>(() => parseBoolean(route.query.isDescending?.toString()) ?? false);
const page = computed<number>(() => parseNumber(route.query.page?.toString()) || 1);
const search = computed<string>(() => route.query.search?.toString() ?? "");
const sort = computed<string>(() => route.query.sort?.toString() ?? "");
const type = computed<PokemonType>(() => (route.query.type?.toString() as PokemonType) ?? "");

const sortOptions = computed<SelectOption[]>(() =>
  orderBy(
    Object.entries(tm(rt("moves.sort.options"))).map(([value, text]) => ({ text, value }) as SelectOption),
    "text",
  ),
);

async function refresh(): Promise<void> {
  const payload: SearchMovesPayload = {
    type: type.value,
    category: category.value,
    ids: [],
    search: {
      terms: search.value
        .split(" ")
        .filter((term) => Boolean(term))
        .map((term) => ({ value: `%${term}%` })),
      operator: "And",
    },
    sort: sort.value ? [{ field: sort.value as MoveSort, isDescending: isDescending.value }] : [],
    skip: (page.value - 1) * count.value,
    limit: count.value,
  };
  isLoading.value = true;
  const now = Date.now();
  timestamp.value = now;
  try {
    const results = await searchMoves(payload);
    if (now === timestamp.value) {
      moves.value = results.items;
      total.value = results.total;
    }
  } catch (e: unknown) {
    handleError(e);
  } finally {
    if (now === timestamp.value) {
      isLoading.value = false;
    }
  }
}

function setQuery(key: string, value: string): void {
  const query = { ...route.query, [key]: value };
  switch (key) {
    case "type":
    case "category":
    case "search":
    case "count":
      query.page = "1";
      break;
  }
  router.replace({ ...route, query });
}

watch(
  () => route,
  (route) => {
    if (route.name === "MoveList") {
      const { query } = route;
      if (!query.page || !query.count) {
        router.replace({
          ...route,
          query: isEmpty(query)
            ? {
                type: "",
                category: "",
                search: "",
                sort: "UpdatedOn",
                isDescending: "true",
                page: 1,
                count: 10,
              }
            : {
                page: 1,
                count: 10,
                ...query,
              },
        });
      } else {
        refresh();
      }
    }
  },
  { deep: true, immediate: true },
);
</script>

<template>
  <main class="container">
    <h1>{{ t("moves.title.list") }}</h1>
    <div class="my-3">
      <TarButton
        class="me-1"
        :disabled="isLoading"
        icon="fas fa-rotate"
        :loading="isLoading"
        :status="t('loading')"
        :text="t('actions.refresh')"
        @click="refresh()"
      />
      <CreateMove :category="category" :type="type" :unique-name="search" @error="handleError" />
    </div>
    <div class="row">
      <PokemonTypeSelect class="col-lg-6" :model-value="type" @update:model-value="setQuery('type', $event ?? '')" />
      <MoveCategorySelect class="col-lg-6" :model-value="category" @update:model-value="setQuery('category', $event ?? '')" />
    </div>
    <div class="row">
      <SearchInput class="col-lg-4" :model-value="search" @update:model-value="setQuery('search', $event ?? '')" />
      <SortSelect
        class="col-lg-4"
        :descending="isDescending"
        :model-value="sort"
        :options="sortOptions"
        @descending="setQuery('isDescending', $event.toString())"
        @update:model-value="setQuery('sort', $event ?? '')"
      />
      <CountSelect class="col-lg-4" :model-value="count" @update:model-value="setQuery('count', ($event ?? 10).toString())" />
    </div>
    <template v-if="moves.length">
      <table class="table table-striped">
        <thead>
          <tr>
            <th scope="col">{{ t("moves.sort.options.UniqueName") }}</th>
            <th scope="col">{{ t("moves.sort.options.DisplayName") }}</th>
            <!-- TODO(fpion): Type -->
            <!-- TODO(fpion): Category -->
            <!-- TODO(fpion): Fewer columns? -->
            <th scope="col">{{ t("moves.sort.options.Accuracy") }}</th>
            <th scope="col">{{ t("moves.sort.options.Power") }}</th>
            <th scope="col">{{ t("moves.sort.options.PowerPoints") }}</th>
            <th scope="col">{{ t("moves.sort.options.UpdatedOn") }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="move in moves" :key="move.id">
            <td>
              <RouterLink :to="{ name: 'MoveEdit', params: { id: move.id } }"> <font-awesome-icon icon="fas fa-edit" />{{ move.uniqueName }} </RouterLink>
            </td>
            <td>{{ move.displayName ?? "—" }}</td>
            <td>{{ move.accuracy ? n(move.accuracy / 100, "percent") : "—" }}</td>
            <td>{{ move.power ?? "—" }}</td>
            <td>{{ move.powerPoints }}</td>
            <td><StatusBlock :actor="move.updatedBy" :date="move.updatedOn" /></td>
          </tr>
        </tbody>
      </table>
      <AppPagination :count="count" :model-value="page" :total="total" @update:model-value="setQuery('page', $event.toString())" />
    </template>
    <p v-else>{{ t("moves.empty") }}</p>
  </main>
</template>
