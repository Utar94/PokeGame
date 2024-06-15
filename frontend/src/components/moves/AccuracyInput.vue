<script setup lang="ts">
import { parsingUtils } from "logitar-vue3-ui";
import { useI18n } from "vue-i18n";

import AppInput from "@/components/shared/AppInput.vue";

const { parseNumber } = parsingUtils;
const { t } = useI18n();

withDefaults(
  defineProps<{
    id?: string;
    label?: string;
    modelValue?: number;
  }>(),
  {
    id: "accuracy",
    label: "moves.accuracy.label",
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
    max="100"
    :model-value="modelValue?.toString()"
    :placeholder="label"
    type="number"
    @update:model-value="onModelValueUpdate"
  >
    <template #append>
      <span class="input-group-text">
        <template v-if="modelValue === 0">{{ t("moves.accuracy.neverMisses") }}</template>
        <template v-else>{{ "%" }}</template>
      </span>
    </template>
  </AppInput>
</template>
