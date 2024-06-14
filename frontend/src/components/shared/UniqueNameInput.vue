<script setup lang="ts">
import AppInput from "./AppInput.vue";

withDefaults(
  defineProps<{
    allowedCharacters?: string;
    id?: string;
    label?: string;
    modelValue?: string;
    required?: boolean | string;
  }>(),
  {
    allowedCharacters: "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_",
    id: "unique-name",
    label: "uniqueName.label",
  },
);

defineEmits<{
  (e: "update:model-value", value?: string): void;
}>();
</script>

<template>
  <AppInput
    floating
    :id="id"
    :label="label"
    max="255"
    :model-value="modelValue"
    :placeholder="label"
    :required="required"
    :rules="{ allowed_characters: allowedCharacters }"
    @update:model-value="$emit('update:model-value', $event)"
  />
</template>
