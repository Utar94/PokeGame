<script setup lang="ts">
import { parsingUtils } from "logitar-vue3-ui";
import { useI18n } from "vue-i18n";

import type { ItemCategory } from "@/types/items";

const { parseNumber } = parsingUtils;
const { t } = useI18n();

const props = defineProps<{
  category: ItemCategory;
  size?: number | string;
}>();
</script>

<template>
  <img :alt="t('items.category.icon', { category })" :src="`/icons/item-categories/${props.category.toLowerCase()}.png`" :width="parseNumber(size)" />
</template>
