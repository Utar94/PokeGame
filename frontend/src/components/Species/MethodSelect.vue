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
  name: 'MethodSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: 'method'
    },
    label: {
      type: String,
      default: 'species.evolutions.method.label'
    },
    placeholder: {
      type: String,
      default: 'species.evolutions.method.placeholder'
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
        Object.entries(this.$i18n.t('species.evolutions.method.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
