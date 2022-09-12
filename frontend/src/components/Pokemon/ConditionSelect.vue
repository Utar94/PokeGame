<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="required"
    :rules="rules"
    :value="value"
    @input="$emit('input', $event)"
  >
    <template #after>
      <slot name="after" />
    </template>
    <slot />
  </form-select>
</template>

<script>
export default {
  name: 'ConditionSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'condition'
    },
    label: {
      type: String,
      default: 'pokemon.condition.label'
    },
    placeholder: {
      type: String,
      default: 'pokemon.condition.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: null
    },
    value: {}
  },
  computed: {
    options() {
      return this.orderBy(
        Object.entries(this.$i18n.t('pokemon.condition.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
