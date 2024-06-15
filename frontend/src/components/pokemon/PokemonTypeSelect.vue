<script setup lang="ts">
import type { SelectOption } from "logitar-vue3-ui";
import { arrayUtils } from "logitar-js";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

import AppSelect from "@/components/shared/AppSelect.vue";
import PokemonTypeIcon from "./PokemonTypeIcon.vue";
import type { PokemonType } from "@/types/pokemon";

const { orderBy } = arrayUtils;
const { rt, tm } = useI18n();

withDefaults(
  defineProps<{
    disabled?: boolean | string;
    id?: string;
    modelValue?: PokemonType;
    required?: boolean | string;
  }>(),
  {
    id: "pokemon-type",
  },
);

const options = computed<SelectOption[]>(() =>
  orderBy(
    Object.entries(tm(rt("pokemon.type.options"))).map(([value, text]) => ({ text, value }) as SelectOption),
    "text",
  ),
);

defineEmits<{
  (e: "update:model-value", value?: PokemonType): void;
}>();
</script>

<template>
  <AppSelect
    :disabled="disabled"
    floating
    :id="id"
    label="pokemon.type.label"
    :model-value="modelValue"
    :options="options"
    placeholder="pokemon.type.placeholder"
    :required="required"
    :show-status="required ? 'touched' : 'never'"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <template #prepend>
      <span v-if="modelValue" class="input-group-text">
        <PokemonTypeIcon size="40" :type="modelValue" />
      </span>
    </template>
  </AppSelect>
</template>
