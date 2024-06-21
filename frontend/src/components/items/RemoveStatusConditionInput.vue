<script setup lang="ts">
import { TarCheckbox } from "logitar-vue3-ui";
import { useI18n } from "vue-i18n";

import AppInput from "@/components/shared/AppInput.vue";

const { t } = useI18n();

withDefaults(
  defineProps<{
    all?: string | boolean;
    id?: string;
    label?: string;
    modelValue?: string;
  }>(),
  {
    id: "remove-status-condition",
    label: "items.properties.medicine.removeStatusCondition",
  },
);

const emit = defineEmits<{
  (e: "all", value?: boolean): void;
  (e: "update:model-value", value?: string): void;
}>();
</script>

<template>
  <AppInput
    :disabled="all"
    floating
    :id="id"
    :label="label"
    min="0"
    max="255"
    :model-value="modelValue"
    step="1"
    :placeholder="label"
    @update:model-value="emit('update:model-value', $event)"
  >
    <template #after>
      <TarCheckbox
        id="remove-all-status-conditions"
        :label="t('items.properties.medicine.removeAllStatusConditions')"
        :model-value="all"
        @update:model-value="$emit('all', $event)"
      />
    </template>
  </AppInput>
</template>
