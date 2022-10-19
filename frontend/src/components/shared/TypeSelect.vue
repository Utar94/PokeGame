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
  name: 'TypeSelect',
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      default: 'type'
    },
    label: {
      type: String,
      default: 'type.label'
    },
    placeholder: {
      type: String,
      default: 'type.placeholder'
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
        Object.entries(this.$i18n.t('type.options'))
          .filter(([value]) => !this.exclude.includes(value))
          .map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
