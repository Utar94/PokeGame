<script setup lang="ts">
import { TarAvatar } from "logitar-vue3-ui";

import type { Item } from "@/types/items";
import { formatItem } from "@/helpers/displayUtils";

defineProps<{
  item: Item;
}>();
</script>

<template>
  <TarAvatar :display-name="formatItem(item)" icon="fas fa-shopping-cart" :url="item.picture" variant="light" />
</template>
