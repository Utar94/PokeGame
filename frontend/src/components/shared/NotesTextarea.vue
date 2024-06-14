<script setup lang="ts">
import AppTextarea from "./AppTextarea.vue";

defineProps<{
  modelValue?: string;
}>();

defineEmits<{
  (e: "update:model-value", value?: string): void;
}>();
</script>

<template>
  <AppTextarea
    floating
    id="notes"
    label="notes"
    :model-value="modelValue"
    placeholder="notes"
    rows="15"
    @update:model-value="$emit('update:model-value', $event)"
  />
</template>
