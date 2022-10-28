<template>
  <form-select
    :disabled="disabled"
    :id="id"
    :label="label"
    :options="options"
    :placeholder="placeholder"
    :required="required"
    :value="value"
    @input="$emit('input', $event)"
  />
</template>

<script>
export default {
  name: 'AbilityTypeSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'type'
    },
    label: {
      type: String,
      default: 'abilities.type.label'
    },
    placeholder: {
      type: String,
      default: 'abilities.type.placeholder'
    },
    required: {
      type: Boolean,
      default: false
    },
    value: {}
  },
  computed: {
    options() {
      return this.orderBy(
        Object.entries(this.$i18n.t('abilities.type.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
