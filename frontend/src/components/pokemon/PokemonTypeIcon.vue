<script setup lang="ts">
import { parsingUtils } from "logitar-vue3-ui";
import { useI18n } from "vue-i18n";

import type { PokemonType } from "@/types/pokemon";

const { parseNumber } = parsingUtils;
const { t } = useI18n();

const props = defineProps<{
  type: PokemonType;
  size?: number | string;
}>();
</script>

<template>
  <img :alt="t('pokemon.type.icon', { type })" :src="`/icons/types/${props.type.toLowerCase()}.png`" :height="parseNumber(size)" :width="parseNumber(size)" />
</template>
