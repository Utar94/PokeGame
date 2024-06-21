<script setup lang="ts">
import { parsingUtils } from "logitar-js";

import AppInput from "@/components/shared/AppInput.vue";

const { parseNumber } = parsingUtils;

withDefaults(
  defineProps<{
    id?: string;
    label?: string;
    modelValue?: number;
  }>(),
  {
    id: "catch-rate-modifier",
    label: "items.properties.pokeBall.catchRateModifier",
  },
);

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
    min="0"
    :model-value="modelValue?.toString()"
    step="0.01"
    :placeholder="label"
    type="number"
    @update:model-value="onModelValueUpdate"
  >
    <template #prepend>
      <span class="input-group-text">{{ "×" }}</span>
    </template>
  </AppInput>
</template>
