<script setup lang="ts">
import { parsingUtils } from "logitar-vue3-ui";

import AppInput from "@/components/shared/AppInput.vue";

const { parseNumber } = parsingUtils;

withDefaults(
  defineProps<{
    id?: string;
    label?: string;
    modelValue?: number;
  }>(),
  {
    id: "power-points",
    label: "moves.powerPoints",
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
    min="1"
    max="40"
    :model-value="modelValue?.toString()"
    :placeholder="label"
    type="number"
    @update:model-value="onModelValueUpdate"
  />
</template>
