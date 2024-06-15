<script setup lang="ts">
import { parsingUtils } from "logitar-vue3-ui";
import { useI18n } from "vue-i18n";

import type { MoveCategory } from "@/types/moves";

const { parseNumber } = parsingUtils;
const { t } = useI18n();

const props = defineProps<{
  category: MoveCategory;
  size?: number | string;
}>();
</script>

<template>
  <img :alt="t('moves.category.icon', { category })" :src="`/icons/move-categories/${props.category.toLowerCase()}.png`" :width="parseNumber(size)" />
</template>
