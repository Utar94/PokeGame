<script setup lang="ts">
import { computed } from "vue";
import { parsingUtils } from "logitar-vue3-ui";

import AppInput from "@/components/shared/AppInput.vue";
import type { PokemonStatistic } from "@/types/pokemon";

const { parseNumber } = parsingUtils;

const props = defineProps<{
  modelValue?: number;
  statistic: PokemonStatistic;
}>();
const id = computed<string>(() => `${props.statistic}-stages`);
const label = computed<string>(() => `pokemon.statistic.options.${props.statistic}`);

const emit = defineEmits<{
  (e: "update:model-value", value?: number): void;
}>();

function onModelValueUpdate(value: string | undefined): void {
  emit("update:model-value", parseNumber(value));
}
</script>

<template>
  <AppInput
    floating
    :id="id"
    :label="label"
    min="-6"
    max="6"
    :model-value="modelValue?.toString()"
    :placeholder="label"
    type="number"
    @update:model-value="onModelValueUpdate"
  />
</template>
