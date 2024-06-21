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
    id: "price",
    label: "items.price",
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
    step="1"
    :placeholder="label"
    type="number"
    @update:model-value="onModelValueUpdate"
  />
</template>
