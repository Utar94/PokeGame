<script setup lang="ts">
import type { SelectOption } from "logitar-vue3-ui";
import { arrayUtils } from "logitar-js";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

import AppSelect from "@/components/shared/AppSelect.vue";
import MoveCategoryIcon from "./MoveCategoryIcon.vue";
import type { MoveCategory } from "@/types/moves";

const { orderBy } = arrayUtils;
const { rt, tm } = useI18n();

withDefaults(
  defineProps<{
    disabled?: boolean | string;
    id?: string;
    modelValue?: MoveCategory;
    required?: boolean | string;
  }>(),
  {
    id: "move-category",
  },
);

const options = computed<SelectOption[]>(() =>
  orderBy(
    Object.entries(tm(rt("moves.category.options"))).map(([value, text]) => ({ text, value }) as SelectOption),
    "text",
  ),
);

defineEmits<{
  (e: "update:model-value", value?: MoveCategory): void;
}>();
</script>

<template>
  <AppSelect
    :disabled="disabled"
    floating
    :id="id"
    label="moves.category.label"
    :model-value="modelValue"
    :options="options"
    placeholder="moves.category.placeholder"
    :required="required"
    :show-status="required ? 'touched' : 'never'"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <template #prepend>
      <span v-if="modelValue" class="input-group-text">
        <MoveCategoryIcon :category="modelValue" size="40" />
      </span>
    </template>
  </AppSelect>
</template>
