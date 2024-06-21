<script setup lang="ts">
import { TarCheckbox } from "logitar-vue3-ui";
import { parsingUtils } from "logitar-js";
import { useI18n } from "vue-i18n";

import AppInput from "@/components/shared/AppInput.vue";

const { parseNumber } = parsingUtils;
const { t } = useI18n();

withDefaults(
  defineProps<{
    allMoves?: string | boolean;
    id?: string;
    label?: string;
    modelValue?: number;
    percentage?: string | boolean;
  }>(),
  {
    id: "restore-power-points",
    label: "items.properties.medicine.restorePowerPoints",
  },
);

const emit = defineEmits<{
  (e: "all-moves", value?: boolean): void;
  (e: "percentage", value?: boolean): void;
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
    :min="percentage || allMoves ? 1 : 0"
    :max="percentage ? 100 : undefined"
    :model-value="modelValue?.toString()"
    step="1"
    :placeholder="label"
    type="number"
    @update:model-value="onModelValueUpdate"
  >
    <template #append v-if="percentage">
      <span class="input-group-text">{{ "%" }}</span>
    </template>
    <template #after>
      <TarCheckbox
        id="is-power-point-percentage"
        :label="t('items.properties.medicine.isPowerPointPercentage')"
        :model-value="percentage"
        @update:model-value="$emit('percentage', $event)"
      />
      <TarCheckbox
        id="restore-all-moves"
        :label="t('items.properties.medicine.restoreAllMoves')"
        :model-value="allMoves"
        @update:model-value="$emit('all-moves', $event)"
      />
    </template>
  </AppInput>
</template>
