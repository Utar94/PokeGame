<template>
  <form-field
    :disabled="disabled"
    :id="id"
    :label="label"
    :minValue="0"
    :required="required"
    :step="0.01"
    type="number"
    :value="value"
    @input="$emit('input', $event)"
  >
    <slot />
    <template #after>
      <slot name="after" />
    </template>
  </form-field>
</template>

<script>
export default {
  name: 'BallModifierField',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'ballModifier'
    },
    label: {
      type: String,
      default: 'items.ballModifier'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  }
}
</script>
