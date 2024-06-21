<script setup lang="ts">
import { TarCheckbox } from "logitar-vue3-ui";
import { parsingUtils } from "logitar-js";
import { useI18n } from "vue-i18n";

import AppInput from "@/components/shared/AppInput.vue";

const { parseNumber } = parsingUtils;
const { t } = useI18n();

withDefaults(
  defineProps<{
    id?: string;
    label?: string;
    modelValue?: number;
    percentage?: string | boolean;
    revive?: string | boolean;
  }>(),
  {
    id: "hit-point-healing",
    label: "items.properties.medicine.hitPointHealing",
  },
);

const emit = defineEmits<{
  (e: "percentage", value?: boolean): void;
  (e: "revive", value?: boolean): void;
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
    :min="percentage || revive ? 1 : 0"
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
        id="is-hit-point-percentage"
        :label="t('items.properties.medicine.isHitPointPercentage')"
        :model-value="percentage"
        @update:model-value="$emit('percentage', $event)"
      />
      <TarCheckbox
        id="does-revive-fainted"
        :label="t('items.properties.medicine.doesReviveFainted')"
        :model-value="revive"
        @update:model-value="$emit('revive', $event)"
      />
    </template>
  </AppInput>
</template>
