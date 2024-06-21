<script setup lang="ts">
import UrlInput from "./UrlInput.vue";

withDefaults(
  defineProps<{
    id?: string;
    label?: string;
    modelValue?: string;
  }>(),
  {
    id: "picture",
    label: "picture",
  },
);

defineEmits<{
  (e: "update:model-value", value?: string): void;
}>();
</script>

<template>
  <UrlInput
    floating
    :id="id"
    :label="label"
    max="2048"
    :model-value="modelValue"
    :placeholder="label"
    @update:model-value="$emit('update:model-value', $event)"
  />
</template>
