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
    id: {
      type: String,
      default: 'category'
    },
    label: {
      type: String,
      default: 'moves.category.label'
    },
    placeholder: {
      type: String,
      default: 'moves.category.placeholder'
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
        Object.entries(this.$i18n.t('moves.category.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  }
}
</script>
