<script setup lang="ts">
import type { SelectOption } from "logitar-vue3-ui";
import { arrayUtils } from "logitar-js";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

import AppSelect from "@/components/shared/AppSelect.vue";
import ItemCategoryIcon from "./ItemCategoryIcon.vue";
import type { ItemCategory } from "@/types/items";

const { orderBy } = arrayUtils;
const { rt, tm } = useI18n();

withDefaults(
  defineProps<{
    disabled?: boolean | string;
    id?: string;
    modelValue?: ItemCategory;
    required?: boolean | string;
  }>(),
  {
    id: "item-category",
  },
);

const options = computed<SelectOption[]>(() =>
  orderBy(
    Object.entries(tm(rt("items.category.options"))).map(([value, text]) => ({ text, value }) as SelectOption),
    "text",
  ),
);

defineEmits<{
  (e: "update:model-value", value?: ItemCategory): void;
}>();
</script>

<template>
  <AppSelect
    :disabled="disabled"
    floating
    :id="id"
    label="items.category.label"
    :model-value="modelValue"
    :options="options"
    placeholder="items.category.placeholder"
    :required="required"
    :show-status="required ? 'touched' : 'never'"
    @update:model-value="$emit('update:model-value', $event)"
  >
    <template #prepend>
      <span v-if="modelValue" class="input-group-text">
        <ItemCategoryIcon :category="modelValue" size="40" />
      </span>
    </template>
  </AppSelect>
</template>
