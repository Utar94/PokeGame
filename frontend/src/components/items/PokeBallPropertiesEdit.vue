<script setup lang="ts">
import { useI18n } from "vue-i18n";

import CatchRateModifierInput from "./CatchRateModifierInput.vue";
import type { PokeBallProperties } from "@/types/items";

const { t } = useI18n();

const props = withDefaults(
  defineProps<{
    modelValue?: PokeBallProperties;
  }>(),
  {
    modelValue: () => ({ catchRateModifier: 0 }),
  },
);

const emit = defineEmits<{
  (e: "update:model-value", value: PokeBallProperties): void;
}>();

function setProperty(key: keyof PokeBallProperties, value: number | undefined): void {
  const properties: PokeBallProperties = { ...props.modelValue };
  switch (key) {
    case "catchRateModifier":
      properties.catchRateModifier = (value as number) ?? false;
      break;
  }
  emit("update:model-value", properties);
}
</script>

<template>
  <div>
    <h3>{{ t("items.properties.label") }}</h3>
    <CatchRateModifierInput :model-value="modelValue.catchRateModifier" @update:model-value="setProperty('catchRateModifier', $event)" />
  </div>
</template>
